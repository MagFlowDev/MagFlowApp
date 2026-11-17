using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Helpers;
using MagFlow.Web.Resources;
using MudBlazor;
using ContractorEntity = MagFlow.Domain.CompanyScope.Contractor;

namespace MagFlow.Web.Pages.Modules.Contractors
{
    public partial class Archive
    {
        private MudDataGrid<ContractorDTO> _archiveGrid = null!;
        private bool _isRestoring;
        private int? _restoringId;

        private async Task<GridData<ContractorDTO>> LoadArchiveAsync(
            GridState<ContractorDTO> state, CancellationToken cancellationToken)
        {
            var sort = state.SortDefinitions.FirstOrDefault();
            var sortBy = sort?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _archiveGrid.RenderedColumns.FirstOrDefault(column => column.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }

            var queryOptions = new QueryOptions<ContractorEntity>
            {
                PageNumber = state.Page,
                PageSize = state.PageSize,
                SortBy = sortBy ?? nameof(ContractorDTO.Id),
                Descending = sort?.Descending == true
            };
            queryOptions.ApplyFilters(state.FilterDefinitions);
            var response = await ContractorService.GetManyAsync(queryOptions, archive: true);

            return new GridData<ContractorDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount
            };
        }

        private async Task RestoreContractorsAsync(ContractorDTO? row = null)
        {
            if (!HasModulePermission("Contractors", PermissionFlags.Edit) || _isRestoring)
                return;

            var contractors = row is null ? _archiveGrid.Selection.ToArray() : new[] { row };
            if (contractors.Length == 0)
                return;

            _isRestoring = true;
            _restoringId = row?.Id;
            try
            {
                var message = contractors.Length == 1
                    ? Localizer[Langs.RestoreContractorPrompt, contractors[0].Name]
                    : Localizer[Langs.RestoreContractorsPrompt, contractors.Length];

                var parameters = new DialogParameters<ConfirmRestoreDialog>
                {
                    { dialog => dialog.ContentText, message.Value }
                };
                var dialog = await DialogService.ShowAsync<ConfirmRestoreDialog>(
                    Localizer[Langs.Restore], parameters);

                if (await dialog.Result is not { Canceled: false } ||
                    !HasModulePermission("Contractors", PermissionFlags.Edit))
                    return;

                var ids = contractors.Select(contractor => contractor.Id).ToArray();
                var result = await ContractorService.RestoreArchivedContractors(ids);
                if (result != Enums.Result.Success)
                {
                    Snackbar.Add(Localizer[Langs.RestoreFailed], Severity.Error);
                    return;
                }

                _archiveGrid.Selection.ExceptWith(contractors);
                await _archiveGrid.ReloadServerData();
                Snackbar.Add(Localizer[Langs.RestoreSuccess], Severity.Success);
            }
            finally
            {
                _restoringId = null;
                _isRestoring = false;
            }
        }
    }
}
