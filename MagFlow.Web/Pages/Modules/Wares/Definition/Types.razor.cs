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


        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteType(ProductTypeDTO type)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(type.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[type.Id] = true;


            }
            finally
            {
                _isBusy = false;
                _loadingDelete[type.Id] = false;
            }
        }
        private async Task DeleteTypes()
        {

        }
    }
}
