using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares
{
    public partial class Archive
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
            var response = await ItemService.GetArchive(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<ItemDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private void OpenItemDetails(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/item/{item.Id}");
        }


        private Dictionary<int, bool> _loadingRestore = [];
        private bool _loadingRestoreMany { get; set; }
        private bool LoadingRestore(int id) => _loadingRestore.TryGetValue(id, out var value) && value;
        private async Task RestoreItem(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_isBusy || (_loadingRestore.TryGetValue(item.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingRestore[item.Id] = true;

                var parameters = new DialogParameters<ConfirmRestoreDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.RestoreWareConfirmation), 1), $"{item.Product?.Name}:{item.Id}") } };
                var dialog = await DialogService.ShowAsync<ConfirmRestoreDialog>(Localizer[Langs.RestoreWareConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.RestoreItem(item);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.RestoreSuccess], Severity.Success);
                        try
                        {
                            _itemsDataGrid.Selection.Remove(item);
                        }
                        catch { }
                        await _itemsDataGrid.ReloadServerData();
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
                _loadingRestore[item.Id] = false;
            }
        }
        private async Task RestoreItems()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_isBusy || _loadingRestoreMany)
                return;

            var items = _itemsDataGrid?.Selection?.ToList();
            if (items == null || !items.Any())
                return;

            if (items.Count == 1)
            {
                var item = items.FirstOrDefault();
                await RestoreItem(item);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingRestoreMany = true;

                var parameters = new DialogParameters<ConfirmRestoreDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.RestoreWareConfirmation), items.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmRestoreDialog>(Localizer[Langs.RestoreWareConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.RestoreItems(items);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.RestoreSuccess], Severity.Success);
                        _itemsDataGrid.Selection.Clear();
                        await _itemsDataGrid.ReloadServerData();
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
