using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace MagFlow.Web.Pages.Modules.Wares.Ware
{
    public partial class Ware : AuthComponentBase
    {
        private ItemDTO? _item { get; set; }

        private SectionsEnums.WareDetailsSection _currentSection = SectionsEnums.WareDetailsSection.GeneralInformation;
        private bool _isMenuOpened = false;

        bool _loadingDelete = false;
        bool _loadingSave = false;
        bool _isBusy = false;

        private bool ShowComponents => (_item?.Components != null && _item.Components.Any(x => x.Component != null)) || 
            (_item?.Product?.Components != null && _item.Product.Components.Any(x => x.Product != null));

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(ItemId) && _item == null)
            {
                if (int.TryParse(ItemId, out var id))
                    _item = await ItemService.GetItem(id);
                if (_item == null)
                    ItemId = string.Empty;
            }
        }

        private async Task RestoreItem()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_item == null || _loadingDelete || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingDelete = true;

                var parameters = new DialogParameters<ConfirmRestoreDialog> { { x => x.ContentText, string.Format(Localizer[Langs.RestoreWareConfirmation], _item?.Product?.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmRestoreDialog>(Localizer[Langs.RestoreWareConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.RestoreItem(_item);
                    if (result == Enums.Result.Success)
                    {
                        NavigationManager.NavigateTo("/");
                        Snackbar.Add(Localizer[Langs.RestoreSuccess], Severity.Success);
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
                _loadingDelete = false;
            }
        }

        private async Task DeleteItem()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_item == null || _loadingDelete || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingDelete = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer[Langs.DeleteWareConfirmation], _item?.Product?.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteWareConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ItemService.DeleteItem(_item);
                    if (result == Enums.Result.Success)
                    {
                        NavigationManager.NavigateTo("/");
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
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
                _loadingDelete = false;
            }
        }

        private async Task SaveChanges()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Edit))
                return;

            if (_item == null || _loadingSave || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingSave = true;

                var tempComponents = new List<ItemComponentDTO>();
                _item.Components.ForEach(x => tempComponents.Add(new ItemComponentDTO() { Component = x.Component, IsRequired = x.IsRequired, Note = x.Note, Quantity = x.Quantity }));
                _item.Components = new List<ItemComponentDTO>();
                var result = await ItemService.UpdateItem(_item);
                _item.Components = tempComponents;
                if (result == MagFlow.Shared.Models.Enums.Result.Success)
                {
                    Snackbar.Add(Localizer[Langs.ChangesSaved], MudBlazor.Severity.Success);
                }
                else
                {
                    Snackbar.Add(Localizer[Langs.ErrorOccured], MudBlazor.Severity.Error);
                }
            }
            finally
            {
                _isBusy = false;
                _loadingSave = false;
            }
        }

        private void OnSectionChanged(SectionsEnums.WareDetailsSection section)
        {
            if (_currentSection == section)
                return;
            _currentSection = section;
            StateHasChanged();
        }
    }
}
