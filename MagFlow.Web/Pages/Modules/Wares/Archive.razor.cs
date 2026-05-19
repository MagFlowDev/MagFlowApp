using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares
{
    public partial class Archive
    {
        MudDataGrid<ItemDTO> _itemsDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<ItemDTO>> ServerReloadItems(GridState<ItemDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _itemsDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(ItemDTO.Id);
            var response = await ItemService.GetArchive(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<ItemDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private void OpenItemDetails(ItemDTO item)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/item/{item.Id}");
        }


        private Dictionary<Guid, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(Guid id) => _loadingDelete.TryGetValue(id, out var value) && value;

        private async Task DeleteItems()
        {

        }
    }
}
