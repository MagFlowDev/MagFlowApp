using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares
{
    public partial class WaresList
    {
        MudDataGrid<ItemDTO> _itemsDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<ItemDTO>> ServerReloadItems(GridState<ItemDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _itemsDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(ItemDTO.Id);
            var response = await ItemService.GetItems(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<ItemDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddItemWizard()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

            NavigationManager.NavigateTo($"/item/add");
        }

        private async Task OpenAddItemWizard(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

            try
            {
                var result = await LocalCacheService.CopyItem(item, typeof(ItemDTO).Name);
                if (result == Enums.Result.Success)
                {
                    NavigationManager.NavigateTo($"/item/add");
                }
            }
            catch { }
        }

        private void OpenItemDetails(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/item/{item.Id}");
        }

        private Dictionary<int, bool> _loadingBlock = [];
        private bool _loadingBlockMany { get; set; }
        private bool _loadingUnblockMany { get; set; }
        private bool LoadingBlock(int id) => _loadingBlock.TryGetValue(id, out var value) && value;
        private async Task BlockItem(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_isBusy || (_loadingBlock.TryGetValue(item.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingBlock[item.Id] = true;

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.BlockConfirmation]) } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.BlockConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.BlockItem(item);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.BlockedSuccessfully], Severity.Success);
                        await _itemsDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.BlockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingBlock[item.Id] = false;
            }
        }
        private async Task BlockItems()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_isBusy || _loadingBlockMany)
                return;

            var items = _itemsDataGrid?.Selection?.ToList();
            if (items == null || !items.Any())
                return;

            if (items.Count == 1)
            {
                var item = items.FirstOrDefault();
                await BlockItem(item);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingBlockMany = true;

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.BlockConfirmation]) } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.BlockConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.BlockItems(items);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.BlockedSuccessfully], Severity.Success);
                        _itemsDataGrid.Selection.Clear();
                        await _itemsDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.BlockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingBlockMany = false;
            }
        }
        private async Task UnblockItem(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_isBusy || (_loadingBlock.TryGetValue(item.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingBlock[item.Id] = true;

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.UnblockConfirmation]) }, { x => x.Unblock, true } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.UnblockConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.BlockItem(item, unblock: true);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.UnblockedSuccessfully], Severity.Success);
                        await _itemsDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.UnblockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingBlock[item.Id] = false;
            }
        }
        private async Task UnblockItems()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_isBusy || _loadingUnblockMany)
                return;

            var items = _itemsDataGrid?.Selection?.ToList();
            if (items == null || !items.Any())
                return;

            if (items.Count == 1)
            {
                var item = items.FirstOrDefault();
                await UnblockItem(item);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingUnblockMany = true;

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.UnblockConfirmation]) }, { x => x.Unblock, true } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.UnblockConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.BlockItems(items, unblock: true);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.UnblockedSuccessfully], Severity.Success);
                        _itemsDataGrid.Selection.Clear();
                        await _itemsDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.UnblockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingUnblockMany = false;
            }
        }


        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteItem(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(item.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[item.Id] = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.DeleteWareConfirmation), 1), $"{item.Product?.Name}:{item.Id}") } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteWareConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.DeleteItem(item);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        try
                        {
                            _itemsDataGrid.Selection.Remove(item);
                        }
                        catch { }
                        await _itemsDataGrid.ReloadServerData();
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
                _loadingDelete[item.Id] = false;
            }
        }
        private async Task DeleteItems()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var items = _itemsDataGrid?.Selection?.ToList();
            if (items == null || !items.Any())
                return;

            if (items.Count == 1)
            {
                var item = items.FirstOrDefault();
                await DeleteItem(item);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.DeleteWareConfirmation), items.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteWareConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.DeleteItems(items);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _itemsDataGrid.Selection.Clear();
                        await _itemsDataGrid.ReloadServerData();
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
    }
}
