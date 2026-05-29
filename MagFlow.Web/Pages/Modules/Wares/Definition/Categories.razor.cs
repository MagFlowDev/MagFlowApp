using MagFlow.BLL.Helpers.Localization;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares.Definition
{
    public partial class Categories
    {
        MudDataGrid<ProductCategoryDTO> _categoriesDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<ProductCategoryDTO>> ServerReloadProducts(GridState<ProductCategoryDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _categoriesDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(ProductCategoryDTO.Id);
            var response = await ProductService.GetCategories(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<ProductCategoryDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddCategoryModal()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

            var dialog = await DialogService.ShowAsync<CategoryModal>(Localizer[Langs.AddCategory]);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
                await _categoriesDataGrid.ReloadServerData();
        }

        private async Task OpenCategoryDetails(ProductCategoryDTO category)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            var parameters = new DialogParameters<CategoryModal> { { x => x.Category, category } };
            var dialog = await DialogService.ShowAsync<CategoryModal>(Localizer[Langs.ProductCategory], parameters);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
            {
                try
                {
                    _categoriesDataGrid.Selection.Remove(category);
                }
                catch { }
                await _categoriesDataGrid.ReloadServerData();
            }
        }


        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteCategory(ProductCategoryDTO category)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(category.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[category.Id] = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.DeleteCategoryConfirmation), 1), category.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteCategoryConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteCategory(category);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        try
                        {
                            _categoriesDataGrid.Selection.Remove(category);
                        }
                        catch { }
                        await _categoriesDataGrid.ReloadServerData();
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
                _loadingDelete[category.Id] = false;
            }
        }
        private async Task DeleteCategories()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var categories = _categoriesDataGrid?.Selection?.ToList();
            if (categories == null || !categories.Any())
                return;

            if (categories.Count == 1)
            {
                var category = categories.FirstOrDefault();
                await DeleteCategory(category);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.DeleteCategoryConfirmation), categories.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteCategoryConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteCategories(categories);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _categoriesDataGrid.Selection.Clear();
                        await _categoriesDataGrid.ReloadServerData();
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
