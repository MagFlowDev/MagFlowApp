using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Warehouses.Warehouse
{
    public partial class Warehouse : AuthComponentBase
    {
        private WarehouseDTO? _warehouse { get; set; }

        private SectionsEnums.WarehouseDetailsSection _currentSection = SectionsEnums.WarehouseDetailsSection.GeneralInformation;
        private bool _isMenuOpened = false;

        bool _loadingDelete = false;
        bool _loadingSave = false;
        bool _isBusy = false;

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(WarehouseId) && _warehouse == null)
            {
                if (int.TryParse(WarehouseId, out var id))
                    _warehouse = await WarehouseService.GetWarehouse(id);
                if (_warehouse == null)
                    WarehouseId = string.Empty;
            }
        }

        private async Task DeleteProduct()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Delete))
                return;

            if (_warehouse == null || _loadingDelete || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingDelete = true;

                //var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer[Langs.DeleteProductConfirmation], _product.Name) } };
                //var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteProductConfirmation], parameters);
                //var confirmation = await dialog.Result;
                //if (confirmation != null && !confirmation.Canceled)
                //{
                //    var result = await ProductService.DeleteProduct(_product);
                //    if (result == Enums.Result.Success)
                //    {
                //        NavigationManager.NavigateTo("/");
                //        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                //    }
                //    else
                //    {
                //        Snackbar.Add(Localizer[Langs.DeleteFailed], Severity.Error);
                //    }
                //}
            }
            finally
            {
                _isBusy = false;
                _loadingDelete = false;
            }
        }

        private async Task SaveChanges()
        {
            if (!HasModulePermission("Warehouses", PermissionFlags.Edit))
                return;

            if (_warehouse == null || _loadingSave || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingSave = true;

                //var result = await ProductService.UpdateProduct(_product);
                //if (result == MagFlow.Shared.Models.Enums.Result.Success)
                //{
                //    Snackbar.Add(Localizer[Langs.ChangesSaved], MudBlazor.Severity.Success);
                //}
                //else
                //{
                //    Snackbar.Add(Localizer[Langs.ErrorOccured], MudBlazor.Severity.Error);
                //}
            }
            finally
            {
                _isBusy = false;
                _loadingSave = false;
            }
        }

        private void OnSectionChanged(SectionsEnums.WarehouseDetailsSection section)
        {
            if (_currentSection == section)
                return;
            _currentSection = section;
            StateHasChanged();
        }
    }
}
