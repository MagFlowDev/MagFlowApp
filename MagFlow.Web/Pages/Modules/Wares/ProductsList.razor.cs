using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models.Enumerators;
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
            var response = await ProductService.GetProducts(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<ProductDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddProductModal()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

        }

        private void OpenProductDetails(ProductDTO product)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/product/{product.Id}");
        }

        private Dictionary<Guid, bool> _loadingBlock = [];
        private bool _loadingBlockMany { get; set; }
        private bool _loadingUnblockMany { get; set; }
        private bool LoadingBlock(Guid id) => _loadingBlock.TryGetValue(id, out var value) && value;

        private async Task BlockProducts()
        {

        }

        private async Task UnblockProducts()
        {

        }


        private Dictionary<Guid, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(Guid id) => _loadingDelete.TryGetValue(id, out var value) && value;

        private async Task DeleteProducts()
        {

        }
    }
}
