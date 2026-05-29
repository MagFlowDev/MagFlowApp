using MagFlow.BLL.Helpers.Localization;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
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

            var dialog = await DialogService.ShowAsync<TypeModal>(Localizer[Langs.AddType]);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
                await _typesDataGrid.ReloadServerData();
        }

        private async Task OpenTypeDetails(ProductTypeDTO type)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;

            var parameters = new DialogParameters<TypeModal> { { x => x.Type, type } };
            var dialog = await DialogService.ShowAsync<TypeModal>(Localizer[Langs.ProductType], parameters);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
            {
                try
                {
                    _typesDataGrid.Selection.Remove(type);
                }
                catch { }
                await _typesDataGrid.ReloadServerData();
            }
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

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.DeleteTypeConfirmation), 1), type.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteTypeConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteType(type);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        try
                        {
                            _typesDataGrid.Selection.Remove(type);
                        }
                        catch { }
                        await _typesDataGrid.ReloadServerData();
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
                _loadingDelete[type.Id] = false;
            }
        }
        private async Task DeleteTypes()
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var types = _typesDataGrid?.Selection?.ToList();
            if (types == null || !types.Any())
                return;

            if (types.Count == 1)
            {
                var type = types.FirstOrDefault();
                await DeleteType(type);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.DeleteTypeConfirmation), types.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteTypeConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ProductService.DeleteTypes(types);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _typesDataGrid.Selection.Clear();
                        await _typesDataGrid.ReloadServerData();
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
