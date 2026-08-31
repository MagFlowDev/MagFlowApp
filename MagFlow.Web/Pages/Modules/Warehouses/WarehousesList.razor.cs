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
    public partial class WarehousesList
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
            var response = await WarehouseService.GetManyAsync(queryOptions);

            return new GridData<WarehouseDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddWarehouseWizard()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Add))
                return;

            NavigationManager.NavigateTo($"/warehouse/add");
        }

        private async Task OpenAddWarehouseWizard(WarehouseDTO warehouse)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Add))
                return;

            try
            {
                var result = await LocalCacheService.CopyItem(warehouse, typeof(WarehouseDTO).Name);
                if (result == Enums.Result.Success)
                {
                    NavigationManager.NavigateTo($"/warehouse/add");
                }
            }
            catch { }
        }

        private void OpenWarehouseDetails(WarehouseDTO warehouse)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/warehouse/{warehouse.Id}");
        }

        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteWarehouse(WarehouseDTO warehouse)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(warehouse.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[warehouse.Id] = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.DeleteWarehouseConfirmation), 1), warehouse.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteWarehouseConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await WarehouseService.DeleteEntity(warehouse);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        try
                        {
                            _warehousesDataGrid.Selection.Remove(warehouse);
                        }
                        catch { }
                        await _warehousesDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.DeleteFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingDelete[warehouse.Id] = false;
            }
        }
        private async Task DeleteWarehouses()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var warehouses = _warehousesDataGrid?.Selection?.ToList();
            if (warehouses == null || !warehouses.Any())
                return;

            if (warehouses.Count == 1)
            {
                var warehouse = warehouses.FirstOrDefault();
                await DeleteWarehouse(warehouse);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.DeleteWarehouseConfirmation), warehouses.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteWarehouseConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await WarehouseService.DeleteEntities(warehouses);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _warehousesDataGrid.Selection.Clear();
                        await _warehousesDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.DeleteFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingDeleteMany = false;
            }
        }

        private bool _loadingChangeStatus { get; set; }
        private async Task ChangeStatus()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
                return;

            if (_isBusy || _loadingChangeStatus)
                return;

            try
            {
                _isBusy = true;
                _loadingChangeStatus = true;


            }
            finally
            {
                _isBusy = false;
                _loadingChangeStatus = false;
            }
        }
    }
}
