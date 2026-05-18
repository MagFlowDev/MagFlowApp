using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models.Enumerators;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares.Definition
{
    public partial class Units
    {
        MudDataGrid<UnitDTO> _unitsDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<UnitDTO>> ServerReloadProducts(GridState<UnitDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _unitsDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(UnitDTO.Id);
            var response = await ProductService.GetUnits(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<UnitDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddUnitModal()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

        }

        private void OpenUnitDetails(UnitDTO unit)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            
        }

        private Dictionary<Guid, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(Guid id) => _loadingDelete.TryGetValue(id, out var value) && value;

        private async Task DeleteUnits()
        {

        }
    }
}
