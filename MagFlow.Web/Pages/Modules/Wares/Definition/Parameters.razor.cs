using MagFlow.BLL.Helpers.Localization;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
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

        private async Task OpenParameterDetails(ParameterDTO parameter)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            var parameters = new DialogParameters<ParameterModal> { { x => x.Parameter, parameter } };
            var dialog = await DialogService.ShowAsync<ParameterModal>(Localizer[Langs.ProductParameter], parameters);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
            {
                try
                {
                    _parametersDataGrid.Selection.Remove(parameter);
                }
                catch { }
                await _parametersDataGrid.ReloadServerData();
            }
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

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.DeleteParameterConfirmation), 1), parameter.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteParameterConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteParameter(parameter);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        try
                        {
                            _parametersDataGrid.Selection.Remove(parameter);
                        }
                        catch { }
                        await _parametersDataGrid.ReloadServerData();
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
                _loadingDelete[parameter.Id] = false;
            }
        }
        private async Task DeleteParameters()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var parameters = _parametersDataGrid?.Selection?.ToList();
            if (parameters == null || !parameters.Any())
                return;

            if (parameters.Count == 1)
            {
                var parameter = parameters.FirstOrDefault();
                await DeleteParameter(parameter);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var dialogParameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.DeleteParameterConfirmation), parameters.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteParameterConfirmation], dialogParameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteParameters(parameters);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _parametersDataGrid.Selection.Clear();
                        await _parametersDataGrid.ReloadServerData();
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
