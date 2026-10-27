using MagFlow.BLL.Services.Interfaces;
using MagFlow.Web.Resources;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Shared.Models.FormModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Warehouses
{
    public partial class PrepareStocktakeDialog : AuthComponentBase
    {
        [Inject] private IWarehouseService WarehouseService { get; set; } = default!;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public int? WarehouseId { get; set; }

        private readonly StocktakeFormModel _model = new();
        private List<WarehouseDTO> _warehouses = [];
        private WarehouseDTO? _warehouse;
        private bool _loading;
        private bool _saving;
        private bool _loadFailed;
        private bool _saveFailed;
        private bool _noWarehouses;
        private bool _disposed;
        private bool Busy => _loading || _saving;
        private bool CanSubmit => !Busy && !_loadFailed && _warehouse != null &&
            _model.Type.HasValue && _model.PlannedDate.HasValue &&
            (_model.SelectedSectorIds.Count > 0 || _model.IncludeItemsWithoutSector);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await Load();
        }

        private async Task Load()
        {
            if (Busy || _disposed)
            {
                return;
            }
            if (WarehouseId.HasValue || _model.WarehouseId > 0)
            {
                await SelectWarehouse(WarehouseId ?? _model.WarehouseId);
                return;
            }
            _loading = true;
            _loadFailed = false;
            _noWarehouses = false;
            try
            {
                var response = await WarehouseService.GetManyAsync(
                    new QueryOptions<MagFlow.Domain.CompanyScope.Warehouse>
                    {
                        PageNumber = 0, PageSize = int.MaxValue,
                        SortBy = nameof(MagFlow.Domain.CompanyScope.Warehouse.Name)
                    });
                if (_disposed)
                {
                    return;
                }
                _warehouses = response.Elements.ToList();
                _noWarehouses = _warehouses.Count == 0;
            }
            catch
            {
                _loadFailed = true;
            }
            finally
            {
                _loading = false;
            }
        }

        private async Task SelectWarehouse(int warehouseId)
        {
            if (Busy || _disposed)
            {
                return;
            }
            _loading = true;
            _loadFailed = false;
            _saveFailed = false;
            _warehouse = null;
            _model.WarehouseId = warehouseId;
            _model.SelectedSectorIds.Clear();
            _model.IncludeItemsWithoutSector = false;
            try
            {
                var warehouse = await WarehouseService.GetWarehouse(warehouseId);
                if (_disposed)
                {
                    return;
                }
                if (warehouse == null || warehouse.RemovedAt.HasValue)
                {
                    _loadFailed = true;
                    return;
                }
                _warehouse = warehouse;
            }
            catch
            {
                _loadFailed = true;
            }
            finally
            {
                _loading = false;
            }
        }

        private void SetSectorSelected(int sectorId, bool selected)
        {
            if (Busy)
            {
                return;
            }
            if (selected)
            {
                if (!_model.SelectedSectorIds.Contains(sectorId))
                {
                    _model.SelectedSectorIds.Add(sectorId);
                }
            }
            else
            {
                _model.SelectedSectorIds.Remove(sectorId);
            }
        }

        private async Task Submit()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Add))
            {
                return;
            }
            if (!CanSubmit || _disposed)
            {
                return;
            }
            _saving = true;
            _saveFailed = false;
            try
            {
                var result = await WarehouseService.PrepareStocktake(_model);
                if (_disposed)
                {
                    return;
                }
                if (result == Enums.Result.Success)
                {
                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    _saveFailed = true;
                }
            }
            catch
            {
                _saveFailed = true;
            }
            finally
            {
                _saving = false;
            }
        }

        private void Cancel() => MudDialog.Cancel();

        public new void Dispose()
        {
            _disposed = true;
            base.Dispose();
        }
    }
}
