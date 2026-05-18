using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models.Enumerators;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares.Definition
{
    public partial class Types
    {
        MudDataGrid<ProductTypeDTO> _typesDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<ProductTypeDTO>> ServerReloadProducts(GridState<ProductTypeDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _typesDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(ProductTypeDTO.Id);
            var response = await ProductService.GetTypes(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<ProductTypeDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddTypeModal()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

        }

        private void OpenTypeDetails(ProductTypeDTO type)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            
        }


        private Dictionary<Guid, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(Guid id) => _loadingDelete.TryGetValue(id, out var value) && value;

        private async Task DeleteTypes()
        {

        }
    }
}
