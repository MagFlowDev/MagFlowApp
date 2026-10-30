using MagFlow.BLL.Helpers;
using MagFlow.Web.Helpers;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Web.Resources;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Constants.Identificators;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Warehouses
{
    public partial class StocktakesList : AuthComponentBase
    {
        [Inject] private IWarehouseService WarehouseService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Parameter] public int? WarehouseId { get; set; }

        private MudDataGrid<StocktakeDTO> _stocktakesDataGrid = default!;
        private bool _openingDialog;
        private bool _disposed;
        private string GridId => WarehouseId.HasValue
            ? DataGridID.WAREHOUSE_STOCKTAKES_LIST : DataGridID.WAREHOUSES_STOCKTAKES_LIST;

        private async Task<GridData<StocktakeDTO>> ServerReloadStocktakes(
            GridState<StocktakeDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _stocktakesDataGrid.RenderedColumns
                    .FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(StocktakeDTO.Code);

            var queryOptions = new QueryOptions<Stocktake>()
            {
                PageNumber = state.Page,
                PageSize = state.PageSize,
                SortBy = sortBy,
                Descending = sortDefinition?.Descending ?? true,
                Includes = query => query
                    .Include(x => x.Warehouse)
                    .Include(x => x.CreatedBy)
            };
            queryOptions.ApplyFilters(state.FilterDefinitions);

            if (WarehouseId.HasValue)
            {
                queryOptions.ColumnFilters ??= [];
                queryOptions.ColumnFilters.Add(ColumnFilter.Create(
                    nameof(Stocktake.WarehouseId),
                    MagFlow.Shared.Models.FilterOperator.Equals,
                    WarehouseId.Value));
            }

            var response = await WarehouseService.GetStocktakes(queryOptions);

            return new GridData<StocktakeDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task PrepareStocktake()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Add))
            {
                return;
            }
            if (_openingDialog || _disposed)
            {
                return;
            }
            _openingDialog = true;
            try
            {
                var parameters = new DialogParameters<PrepareStocktakeDialog>
                {
                    { x => x.WarehouseId, WarehouseId }
                };
                var dialog = await DialogService.ShowAsync<PrepareStocktakeDialog>(
                    Localizer[Langs.StocktakePrepareTitle], parameters,
                    new DialogOptions
                    {
                        CloseButton = false, BackdropClick = false, CloseOnEscapeKey = false,
                        FullWidth = true, MaxWidth = MaxWidth.Small
                    });
                var result = await dialog.Result;
                if (!_disposed && result?.Canceled == false)
                {
                    Snackbar.Add(Localizer[Langs.StocktakePrepared], Severity.Success);
                    await Refresh();
                }
            }
            finally
            {
                _openingDialog = false;
            }
        }

        private void OpenStocktake(StocktakeDTO stocktake)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Read))
            {
                return;
            }
            NavigationManager.NavigateTo($"/stocktake/{stocktake.Id}");
        }

        private static Color StocktakeStatusColor(Enums.EntityStatus status) => status switch
        {
            Enums.EntityStatus.InProgress => Color.Info,
            Enums.EntityStatus.AwaitingApproval => Color.Warning,
            Enums.EntityStatus.Confirmed => Color.Success,
            Enums.EntityStatus.Canceled => Color.Error,
            _ => Color.Default
        };

        private Task Refresh() => _stocktakesDataGrid.ReloadServerData();

        public new void Dispose()
        {
            _disposed = true;
            base.Dispose();
        }
    }
}
