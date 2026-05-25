using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Resources;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares.Definition
{
    public partial class Parameters
    {
        MudDataGrid<ParameterDTO> _parametersDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<ParameterDTO>> ServerReloadProducts(GridState<ParameterDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _parametersDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(ParameterDTO.Id);
            var response = await ProductService.GetParameters(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<ParameterDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddParameterModal()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Add))
                return;

            var dialog = await DialogService.ShowAsync<ParameterModal>(Localizer[Langs.AddParameter]);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
                await _parametersDataGrid.ReloadServerData();
        }

        private void OpenParameterDetails(ParameterDTO parameter)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            
        }


        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteParameter(ParameterDTO parameter)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(parameter.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[parameter.Id] = true;


            }
            finally
            {
                _isBusy = false;
                _loadingDelete[parameter.Id] = false;
            }
        }
        private async Task DeleteParameters()
        {

        }
    }
}
