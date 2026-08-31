using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Helpers;
using MagFlow.Web.Resources;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Warehouses
{
    public partial class Archive
    {
        MudDataGrid<WarehouseDTO> _warehousesDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<WarehouseDTO>> ServerReloadWarehouses(GridState<WarehouseDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _warehousesDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(WarehouseDTO.Id);
            var queryOptions = new MagFlow.Shared.Models.QueryOptions<MagFlow.Domain.CompanyScope.Warehouse>()
            {
                PageNumber = state.Page,
                PageSize = state.PageSize,
                SortBy = sortBy,
                Descending = sortDefinition?.Descending == true,
                Includes = warehouse => warehouse
                    .Include(x => x.Sectors).ThenInclude(y => y.Rows).ThenInclude(z => z.Slots)
            };
            queryOptions.ApplyFilters(state.FilterDefinitions);
            var response = await WarehouseService.GetManyAsync(queryOptions, archive: true);

            return new GridData<WarehouseDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }



        private void OpenWarehouseDetails(WarehouseDTO warehouse)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/warehouse/{warehouse.Id}");
        }

        private Dictionary<int, bool> _loadingRestore = [];
        private bool _loadingRestoreMany { get; set; }
        private bool LoadingRestore(int id) => _loadingRestore.TryGetValue(id, out var value) && value;
        private async Task RestoreWarehouse(WarehouseDTO warehouse)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
                return;

            if (_isBusy || (_loadingRestore.TryGetValue(warehouse.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingRestore[warehouse.Id] = true;

                var parameters = new DialogParameters<ConfirmRestoreDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.RestoreWarehouseConfirmation), 1), warehouse.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmRestoreDialog>(Localizer[Langs.RestoreWarehouseConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await WarehouseService.RestoreEntity(warehouse);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.RestoreSuccess], Severity.Success);
                        try
                        {
                            _warehousesDataGrid.Selection.Remove(warehouse);
                        }
                        catch { }
                        await _warehousesDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.RestoreFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingRestore[warehouse.Id] = false;
            }
        }
        private async Task RestoreWarehouses()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
                return;

            if (_isBusy || _loadingRestoreMany)
                return;

            var warehouses = _warehousesDataGrid?.Selection?.ToList();
            if (warehouses == null || !warehouses.Any())
                return;

            if (warehouses.Count == 1)
            {
                var warehouse = warehouses.FirstOrDefault();
                await RestoreWarehouse(warehouse);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingRestoreMany = true;

                var parameters = new DialogParameters<ConfirmRestoreDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.RestoreWarehouseConfirmation), warehouses.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmRestoreDialog>(Localizer[Langs.RestoreWarehouseConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await WarehouseService.RestoreEntities(warehouses);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.RestoreSuccess], Severity.Success);
                        _warehousesDataGrid.Selection.Clear();
                        await _warehousesDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.RestoreFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingRestoreMany = false;
            }
        }
    }
}
