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
        [Parameter] public int? WarehouseId { get; set; }

        private MudDataGrid<StocktakeDTO> _stocktakesDataGrid = default!;
        private bool _openingDialog;
        private bool _loadFailed;
        private bool _disposed;
        private string GridId => WarehouseId.HasValue
            ? DataGridID.WAREHOUSE_STOCKTAKES_LIST : DataGridID.WAREHOUSES_STOCKTAKES_LIST;

        private async Task<GridData<StocktakeDTO>> ServerReloadStocktakes(
            GridState<StocktakeDTO> state, CancellationToken token)
        {
            _loadFailed = false;
            var sort = state.SortDefinitions.FirstOrDefault();
            var column = sort == null ? null : _stocktakesDataGrid.RenderedColumns
                .FirstOrDefault(x => x.PropertyName == sort.SortBy);
            var options = new QueryOptions<Stocktake>
            {
                PageNumber = state.Page,
                PageSize = state.PageSize,
                SortBy = column?.Tag?.ToString()?.Split('|')[0].Trim() ?? nameof(Stocktake.CreatedAt),
                Descending = sort?.Descending ?? true,
                Includes = query => query.Include(x => x.Warehouse).Include(x => x.CreatedBy)
            };
            options.ApplyFilters(state.FilterDefinitions);
            if (WarehouseId.HasValue)
            {
                options.ColumnFilters ??= [];
                options.ColumnFilters.Add(ColumnFilter.Create(
                    nameof(Stocktake.WarehouseId), MagFlow.Shared.Models.FilterOperator.Equals, WarehouseId.Value));
            }
            try
            {
                var response = await WarehouseService.GetStocktakes(options);
                token.ThrowIfCancellationRequested();
                return new GridData<StocktakeDTO>
                {
                    Items = response.Elements, TotalItems = response.TotalCount
                };
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch
            {
                if (!_disposed)
                {
                    _loadFailed = true;
                }
                return new GridData<StocktakeDTO> { Items = [], TotalItems = 0 };
            }
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

        private Task Refresh() => _stocktakesDataGrid.ReloadServerData();

        public new void Dispose()
        {
            _disposed = true;
            base.Dispose();
        }
    }
}
