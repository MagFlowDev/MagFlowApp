using WarehouseEntity = MagFlow.Domain.CompanyScope.Warehouse;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;

namespace MagFlow.Web.Pages.Modules.Warehouses
{
    public partial class LocationList
    {
        private List<WarehouseDTO> _warehouses = [];
        private WarehouseDTO? _warehouse;
        private int _warehouseId;
        private int _requestVersion;
        private bool _refreshing;
        private bool _loading;
        private bool _loadFailed;
        private bool _disposed;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await Refresh();
        }

        private async Task Refresh()
        {
            if (_refreshing || _disposed)
                return;

            _refreshing = true;
            _loadFailed = false;
            var request = ++_requestVersion;
            try
            {
                var response = await WarehouseService.GetManyAsync(new QueryOptions<WarehouseEntity>
                {
                    PageNumber = 0,
                    PageSize = int.MaxValue,
                    SortBy = nameof(WarehouseEntity.Name)
                });
                if (_disposed || request != _requestVersion)
                    return;

                _warehouses = response.Elements.ToList();
                var selected = _warehouses.FirstOrDefault(x => x.Id == _warehouseId)
                    ?? _warehouses.FirstOrDefault();
                if (selected == null)
                {
                    _warehouseId = 0;
                    _warehouse = null;
                    _loading = false;
                    return;
                }

                await SelectWarehouse(selected.Id);
            }
            catch
            {
                if (!_disposed)
                    _loadFailed = true;
            }
            finally
            {
                if (!_disposed)
                    _refreshing = false;
            }
        }

        private async Task SelectWarehouse(int warehouseId)
        {
            if (_disposed || !_warehouses.Any(x => x.Id == warehouseId))
                return;

            var request = ++_requestVersion;
            _warehouseId = warehouseId;
            _warehouse = null;
            _loading = true;
            _loadFailed = false;
            try
            {
                var warehouse = await WarehouseService.GetWarehouse(warehouseId);
                if (_disposed || request != _requestVersion)
                    return;

                if (warehouse == null || warehouse.RemovedAt.HasValue)
                {
                    _loadFailed = true;
                    return;
                }

                _warehouse = warehouse;
            }
            catch
            {
                if (!_disposed && request == _requestVersion)
                    _loadFailed = true;
            }
            finally
            {
                if (!_disposed && request == _requestVersion)
                    _loading = false;
            }
        }

        public new void Dispose()
        {
            _disposed = true;
            ++_requestVersion;
            base.Dispose();
        }
    }
}
