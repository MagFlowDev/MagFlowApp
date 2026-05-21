using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Resources;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares.Definition
{
    public partial class Units
    {
        MudDataGrid<UnitDTO> _unitsDataGrid;

        private HashSet<int> _selectedIds = new();
        private HashSet<int> _visibleIds = new();
        private List<UnitDTO> _loadedRoots = [];

        string? _searchString = null;
        bool _isBusy = false;


        private async Task<GridData<UnitDTO>> ServerReloadUnits(GridState<UnitDTO> state, CancellationToken token)
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

            _loadedRoots = response?.Elements?.ToList() ?? new List<UnitDTO>();
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

            var dialog = await DialogService.ShowAsync<AddUnitModal>(Localizer[Langs.AddMeasurement]);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
                await _unitsDataGrid.ReloadServerData();
        }

        private void OpenUnitDetails(UnitDTO unit)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Read))
                return;
        }

        private bool IsSelected(UnitDTO unit)
            => _selectedIds.Contains(unit.Id);

        private void ToggleSelection(UnitDTO unit, bool selected)
        {
            if (selected)
                _selectedIds.Add(unit.Id);
            else
                _selectedIds.Remove(unit.Id);
        }

        private IEnumerable<UnitDTO> VisibleUnits
            => _loadedRoots.SelectMany(Flatten);

        private IEnumerable<UnitDTO> Flatten(UnitDTO unit)
        {
            yield return unit;

            foreach (var child in unit.RelatedUnits ?? Enumerable.Empty<UnitDTO>())
            {
                foreach (var x in Flatten(child))
                    yield return x;
            }
        }

        private bool? SelectAllStates()
        {
            var visible = VisibleUnits.ToList();

            if (visible.Count == 0)
                return false;

            var selected = visible.Count(x => _selectedIds.Contains(x.Id));

            if (selected == 0)
                return false;

            if (selected == visible.Count)
                return true;

            return null;
        }

        private void OnSelectAllChanged(bool? value)
        {
            var visible = VisibleUnits;

            if (value == true)
            {
                foreach (var u in visible)
                    _selectedIds.Add(u.Id);
            }
            else
            {
                foreach (var u in visible)
                    _selectedIds.Remove(u.Id);
            }
        }

        private Dictionary<int, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
        private bool LoadingDelete(int id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteUnit(UnitDTO unit)
        {
            if (!HasModulePermission("Wares", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(unit.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[unit.Id] = true;

                
            }
            finally
            {
                _isBusy = false;
                _loadingDelete[unit.Id] = false;
            }
        }
        private async Task DeleteUnits()
        {

        }
    }
}
