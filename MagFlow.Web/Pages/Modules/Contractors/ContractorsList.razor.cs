using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Helpers;
using MagFlow.Web.Resources;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Contractors
{
    public partial class ContractorsList
    {
        MudDataGrid<ContractorDTO> _contractorsDataGrid;
        bool _isBusy = false;
        private Enums.EntityStatus _targetStatus = Enums.EntityStatus.Inactive;
        private bool IsActivationSelected => _targetStatus == Enums.EntityStatus.Active;
        private bool StatusChangeDisabled => _isBusy || _contractorsDataGrid?.Selection?.Any() != true;

        private void ToggleStatusAction()
        {
            _targetStatus = IsActivationSelected
                ? Enums.EntityStatus.Inactive
                : Enums.EntityStatus.Active;
        }

        private async Task ChangeSelectedStatuses(Enums.EntityStatus status)
        {
            if (!HasModulePermission("Contractors", PermissionFlags.Edit) || StatusChangeDisabled)
                return;

            var ids = _contractorsDataGrid.Selection.Select(contractor => contractor.Id).ToArray();
            _isBusy = true;
            try
            {
                var result = await ContractorService.ChangeStatuses(ids, status);
                if (result != Enums.Result.Success)
                {
                    Snackbar.Add(Localizer[Langs.FailedUpdate], Severity.Error);
                    return;
                }

                _contractorsDataGrid.Selection.Clear();
                await _contractorsDataGrid.ReloadServerData();
                Snackbar.Add(Localizer[Langs.ChangesSaved], Severity.Success);
            }
            finally
            {
                _isBusy = false;
            }
        }

        private async Task<GridData<ContractorDTO>> ServerReloadContractors(GridState<ContractorDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _contractorsDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(ContractorDTO.Id);
            var queryOptions = new MagFlow.Shared.Models.QueryOptions<MagFlow.Domain.CompanyScope.Contractor>()
            {
                PageNumber = state.Page,
                PageSize = state.PageSize,
                SortBy = sortBy,
                Descending = sortDefinition?.Descending == true
            };
            queryOptions.ApplyFilters(state.FilterDefinitions);
            var response = await ContractorService.GetManyAsync(queryOptions);

            return new GridData<ContractorDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddContractorDialog()
        {
            if (!HasModulePermission("Contractors", PermissionFlags.Add))
                return;

            var dialog = await DialogService.ShowAsync<AddContractorDialog>(
                Localizer[Langs.AddContractor],
                new DialogOptions
                {
                    FullWidth = true,
                    MaxWidth = MaxWidth.ExtraSmall,
                    BackdropClick = false,
                    CloseOnEscapeKey = false
                });

            if (await dialog.Result is { Canceled: false, Data: true })
                await _contractorsDataGrid.ReloadServerData();
        }

        private void OpenContractorDetails(ContractorDTO contractor)
        {
            if (!HasModulePermission("Contractors", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/contractor/{contractor.Id}");
        }

        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteContractor(ContractorDTO contractor)
        {
            if (!HasModulePermission("Contractors", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(contractor.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[contractor.Id] = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer.GetConfirmationMessage(nameof(Langs.DeleteContractorConfirmation), 1), contractor.Name) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteContractorConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ContractorService.DeleteEntity(contractor);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        try
                        {
                            _contractorsDataGrid.Selection.Remove(contractor);
                        }
                        catch { }
                        await _contractorsDataGrid.ReloadServerData();
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
                _loadingDelete[contractor.Id] = false;
            }
        }
        private async Task DeleteContractors()
        {
            if (!HasModulePermission("Contractors", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var contractors = _contractorsDataGrid?.Selection?.ToList();
            if (contractors == null || !contractors.Any())
                return;

            if (contractors.Count == 1)
            {
                var contractor = contractors.FirstOrDefault();
                await DeleteContractor(contractor);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, Localizer.GetConfirmationMessage(nameof(Langs.DeleteContractorConfirmation), contractors.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteContractorConfirmation], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await ContractorService.DeleteEntities(contractors);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _contractorsDataGrid.Selection.Clear();
                        await _contractorsDataGrid.ReloadServerData();
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
