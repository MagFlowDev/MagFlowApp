using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares.Ware
{
    public partial class Product : AuthComponentBase
    {
        private ProductDTO? _product { get; set; }

        private SectionsEnums.ProductDetailsSection _currentSection = SectionsEnums.ProductDetailsSection.GeneralInformation;
        private bool _isMenuOpened = false;

        bool _loadingDelete = false;
        bool _loadingSave = false;
        bool _isBusy = false;

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(ProductId) && _product == null)
            {
                if (int.TryParse(ProductId, out var id))
                    _product = await ProductService.GetProduct(id);
                if (_product == null)
                    ProductId = string.Empty;
            }
        }

        private async Task DeleteProduct()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_product == null || _loadingDelete || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingDelete = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer[Langs.DeleteProductConfirmation], _product.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteProductConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteProduct(_product);
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

            if (_product == null || _loadingSave || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingSave = true;

                var result = await ProductService.UpdateProduct(_product);
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

        private void OnSectionChanged(SectionsEnums.ProductDetailsSection section)
        {
            if (_currentSection == section)
                return;
            _currentSection = section;
            StateHasChanged();
        }
    }
}
