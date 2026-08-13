using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Helpers;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares
{
    public partial class ProductsList
    {
        MudDataGrid<ProductDTO> _productsDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<ProductDTO>> ServerReloadProducts(GridState<ProductDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _productsDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(ProductDTO.Id);
            var queryOptions = new MagFlow.Shared.Models.QueryOptions<MagFlow.Domain.CompanyScope.Product>()
            {
                PageNumber = state.Page,
                PageSize = state.PageSize,
                SortBy = sortBy,
                Descending = sortDefinition?.Descending == true
            };
            queryOptions.ApplyFilters(state.FilterDefinitions);
            var response = await ProductService.GetProducts(queryOptions);

            return new GridData<ProductDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddProductWizard()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

            NavigationManager.NavigateTo($"/product/add");
        }

        private async Task OpenAddProductWizard(ProductDTO product)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

            try
            {
                var result = await LocalCacheService.CopyItem(product, typeof(ProductDTO).Name);
                if (result == Enums.Result.Success)
                {
                    NavigationManager.NavigateTo($"/product/add");
                }
            }
            catch { }
        }

        private void OpenProductDetails(ProductDTO product)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/product/{product.Id}");
        }


        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteProduct(ProductDTO product)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(product.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[product.Id] = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.DeleteProductConfirmation), 1), product.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteProductConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteProduct(product);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        try
                        {
                            _productsDataGrid.Selection.Remove(product);    
                        }
                        catch { }
                        await _productsDataGrid.ReloadServerData();
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
                _loadingDelete[product.Id] = false;
            }
        }

        private async Task DeleteProducts()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var products = _productsDataGrid?.Selection?.ToList();
            if (products == null || !products.Any())
                return;

            if (products.Count == 1)
            {
                var product = products.FirstOrDefault();
                await DeleteProduct(product);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.DeleteProductConfirmation), products.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteProductConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteProducts(products);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _productsDataGrid.Selection.Clear();
                        await _productsDataGrid.ReloadServerData();
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
