using System.Text.Json;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Warehouses
{
    public partial class StocktakeDetails : AuthComponentBase
    {
        [Parameter] public int StocktakeId { get; set; }
        [Inject] private IWarehouseService WarehouseService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        private StocktakeDetailsDTO? _details;
        private int _loadedId;
        private bool _loading;
        private bool _busy;
        private bool _operationFailed;
        private bool _disposed;
        private int _editingCells;
        private readonly HashSet<int> _expandedItems = [];
        private bool EditableStage => _details?.Stocktake.Status is
            Enums.EntityStatus.Prepared or Enums.EntityStatus.InProgress;
        private bool CanEdit => EditableStage && !_busy &&
            HasModulePermission("Warehouses", PermissionFlags.Edit);

        protected override async Task OnParametersSetAsync()
        {
            if (_loadedId != StocktakeId)
            {
                _details = null;
                _editingCells = 0;
                _expandedItems.Clear();
                await Load();
            }
        }

        private async Task Load()
        {
            var id = StocktakeId;
            _loading = _details == null;
            try
            {
                var details = await WarehouseService.GetStocktakeDetails(id);
                if (_disposed || id != StocktakeId)
                {
                    return;
                }
                _loadedId = id;
                if (_details == null || details == null)
                {
                    _details = details;
                }
                else
                {
                    // Keep row identities and expanded parameter sections after autosave.
                    _details.Stocktake = details.Stocktake;
                    foreach (var item in _details.Items)
                    {
                        var updated = details.Items.Single(x => x.Id == item.Id);
                        item.CheckStatus = updated.CheckStatus;
                        item.Values = updated.Values;
                        item.OriginalQuantity = updated.OriginalQuantity;
                        item.Parameters = updated.Parameters;
                        item.ProductStatus = updated.ProductStatus;
                    }
                }
            }
            catch
            {
                if (!_disposed && id == StocktakeId)
                {
                    _details = null;
                }
            }
            finally
            {
                if (id == StocktakeId)
                {
                    _loading = false;
                }
            }
        }

        private int Count(Enums.StocktakeItemCheckStatus status) =>
            _details?.Items.Count(x => x.CheckStatus == status) ?? 0;

        private void EditingChanged(bool editing)
        {
            if (!_disposed)
            {
                _editingCells = Math.Max(0, _editingCells + (editing ? 1 : -1));
            }
        }

        private async Task SaveValues(StocktakeItemDTO item, Action<StocktakeItemValues> update)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
            {
                return;
            }
            if (!CanEdit)
            {
                return;
            }
            var values = JsonSerializer.Deserialize<StocktakeItemValues>(JsonSerializer.Serialize(item.Values))!;
            update(values);
            await Write(id => WarehouseService.SaveStocktakeItem(id, item.Id, values, null));
        }

        private async Task Mark(StocktakeItemDTO item, Enums.StocktakeItemCheckStatus mark)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
            {
                return;
            }
            if (!CanEdit || _editingCells > 0)
            {
                return;
            }
            await Write(id => WarehouseService.SaveStocktakeItem(id, item.Id, null, mark));
        }

        private async Task FinishCounting()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
            {
                return;
            }
            if (!CanEdit || _editingCells > 0)
            {
                return;
            }
            await Write(WarehouseService.FinishStocktake);
        }

        private async Task ResumeEditing()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
            {
                return;
            }
            await Write(WarehouseService.ResumeStocktake);
        }

        private async Task Approve()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
            {
                return;
            }
            await Write(WarehouseService.ApproveStocktake);
        }

        private async Task DeleteOrCancel(bool delete)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Delete))
                return;

            if (_busy || _disposed || _editingCells > 0 || _details == null)
                return;

            var status = _details.Stocktake.Status;
            if (delete ? status != Enums.EntityStatus.Prepared
                : status is not (Enums.EntityStatus.InProgress or Enums.EntityStatus.AwaitingApproval))
                return;

            var id = StocktakeId;
            _busy = true;
            _operationFailed = false;
            try
            {
                var parameters = new DialogParameters<StocktakeCloseDialog>
                {
                    { x => x.DeleteMode, delete },
                    { x => x.StocktakeCode, _details.Stocktake.Code }
                };
                var dialog = await DialogService.ShowAsync<StocktakeCloseDialog>(
                    Localizer[delete ? Langs.Delete : Langs.StocktakeCancel], parameters,
                    new DialogOptions { FullWidth = true, MaxWidth = MaxWidth.Small, CloseButton = true });
                var confirmation = await dialog.Result;
                if (confirmation?.Canceled != false || _disposed || id != StocktakeId)
                    return;

                if (!HasModulePermission("Warehouses", PermissionFlags.Delete))
                    return;

                var result = delete ? await WarehouseService.DeleteStocktake(id)
                    : await WarehouseService.CancelStocktake(id);
                if (_disposed || id != StocktakeId)
                    return;

                if (result != Enums.Result.Success)
                {
                    Snackbar.Add(Localizer[delete ? Langs.DeleteFailed : Langs.StocktakeCancelFailed], Severity.Error);
                    await Load();
                    return;
                }

                Snackbar.Add(Localizer[delete ? Langs.DeleteSuccess : Langs.StocktakeCanceled], Severity.Success);
                if (delete)
                    NavigationManager.NavigateTo("/");
                else
                    await Load();
            }
            catch
            {
                if (!_disposed && id == StocktakeId)
                    Snackbar.Add(Localizer[delete ? Langs.DeleteFailed : Langs.StocktakeCancelFailed], Severity.Error);
            }
            finally
            {
                _busy = false;
            }
        }

        private async Task Write(Func<int, Task<Enums.Result>> operation)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
            {
                return;
            }
            if (_busy || _disposed)
            {
                return;
            }
            var id = StocktakeId;
            _busy = true;
            _operationFailed = false;
            try
            {
                var result = await operation(id);
                if (!_disposed && id == StocktakeId)
                {
                    _operationFailed = result != Enums.Result.Success;
                    await Load();
                }
            }
            catch
            {
                _operationFailed = true;
            }
            finally
            {
                _busy = false;
            }
        }

        private void ToggleParameters(int itemId)
        {
            if (_busy || _editingCells > 0 || !EditableStage)
            {
                return;
            }
            if (!_expandedItems.Add(itemId))
            {
                _expandedItems.Remove(itemId);
            }
        }

        private static bool IsParameterChanged(StocktakeItemDTO item, StocktakeParameterDTO parameter) =>
            item.Values.Parameters[parameter.Id] != parameter.OriginalValue;

        private async Task RestoreParameter(StocktakeItemDTO item, StocktakeParameterDTO parameter)
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
            {
                return;
            }
            if (!CanEdit || _editingCells > 0)
            {
                return;
            }
            await SaveValues(item, values => values.Parameters[parameter.Id] = parameter.OriginalValue);
        }

        private static string ItemRowClass(StocktakeItemDTO item, int index) => index % 2 == 0
            ? "stocktake-data-row-shaded" : string.Empty;

        private static string StatusIcon(Enums.StocktakeItemCheckStatus status) => status switch
        {
            Enums.StocktakeItemCheckStatus.Checked => Icons.Material.Outlined.CheckCircle,
            Enums.StocktakeItemCheckStatus.Corrected => Icons.Material.Outlined.EditNote,
            Enums.StocktakeItemCheckStatus.Missing => Icons.Material.Outlined.VisibilityOff,
            _ => Icons.Material.Outlined.RadioButtonUnchecked
        };
        private static Color StatusColor(Enums.StocktakeItemCheckStatus status) => status switch
        {
            Enums.StocktakeItemCheckStatus.Checked => Color.Success,
            Enums.StocktakeItemCheckStatus.Corrected => Color.Info,
            Enums.StocktakeItemCheckStatus.Missing => Color.Warning,
            _ => Color.Default
        };

        public new void Dispose()
        {
            _disposed = true;
            base.Dispose();
        }
    }
}
