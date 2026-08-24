using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Helpers;
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

        }
        private async Task DeleteWarehouses()
        {

        }

        private bool _loadingChangeStatus { get; set; }
        private async Task ChangeStatus()
        {

        }
    }
}
