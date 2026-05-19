using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models.Enumerators;
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

        }

        private void OpenCategoryDetails(ProductCategoryDTO category)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            
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


            }
            finally
            {
                _isBusy = false;
                _loadingDelete[category.Id] = false;
            }
        }
        private async Task DeleteCategories()
        {

        }
    }
}
