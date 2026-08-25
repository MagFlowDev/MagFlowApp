using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Web.Components.Dialogs;
using MagFlow.Web.Components.TreeViews;
using MagFlow.Web.Components.Wizards;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace MagFlow.Web.Pages.Modules.Warehouses
{
    public partial class WarehouseWizard : StepperWizardBase<WarehouseFormModel>
    {
        [Inject] public ILocalCacheService LocalCacheService { get; set; } = default!;
        [Inject] public IServiceProvider Services { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private List<TreeItemData<string>> _warehouseTreeItems { get; set; } = new();
        private IReadOnlyCollection<string> _selectedValues { get; set; }

        protected override async Task OnInitializedAsync()
        {
            base.SetServices(LocalCacheService, Services, JS, Snackbar, NavigationManager);
            await base.OnInitializedAsync();

            _stepSections = new()
            {
                [0] = () => _model.GeneralInformation,
                [1] = () => _model.Sectors
            };

            try
            {
                var copiedItem = await LocalCacheService.PasteItem();
                var data = copiedItem.Item1;
                var dataType = copiedItem.Item2;
                if (data != null && dataType == typeof(WarehouseDTO).Name && data is JsonElement element)
                {
                    var product = element.Deserialize<WarehouseDTO>();
                    if (product != null)
                        CreateCopy(product);
                }
            }
            catch { }
        }

        private void CreateCopy(WarehouseDTO dto)
        {
            _model.GeneralInformation.Name = dto.Name;

            _model.Sectors = new List<SectorDTO>();
            dto.Sectors?.ForEach(sector =>
            {
                var newSector = new WarehouseTreeViewTempItem(sector.Name, Enums.WarehouseStorageType.Sector);
                sector.Rows?.ForEach(row =>
                {
                    var newRow = new WarehouseTreeViewTempItem(row.Name, Enums.WarehouseStorageType.Row, newSector.TempId);
                    row.Slots?.ForEach(slot =>
                    {
                        var newSlot = new WarehouseTreeViewTempItem(slot.Name, Enums.WarehouseStorageType.Slot, newRow.TempId);
                        newRow.AddChildren(newSlot);
                    });
                    newSector.AddChildren(newRow);
                });
                _warehouseTreeItems.Add(newSector);
            });
        }

        protected override async Task Save()
        {
            if (_isBusy)
                return;

            var sectors = _warehouseTreeItems
                .Select(x => ((WarehouseTreeViewTempItem)x).GetSectorDTO())
                .Where(x => x != null).Select(x => x!)
                .ToList();
            _model.Sectors = sectors;

            var step = _stepper.Steps[_step];
            if (step == null || !await ValidateStep(_step))
                return;
            await step.SetCompletedAsync(true);

            try
            {
                _isBusy = true;
                _loading = true;

                var result = await WarehouseService.AddWarehouse((WarehouseFormModel)_model);
                if (result == Enums.Result.Success)
                {
                    NavigationManager.NavigateTo("/");
                    Snackbar.Add(Localizer[Langs.ActionSucceed], Severity.Success);
                    return;
                }
                else
                {
                    Snackbar.Add(Localizer[Langs.ErrorOccured], Severity.Error);
                }
            }
            finally
            {
                _isBusy = false;
                _loading = false;
            }
        }

        private void AddSector()
        {
            var sectorBaseName = Localizer[Langs.Sector];
            var sectorNumber = _warehouseTreeItems.Count + 1;
            while(_warehouseTreeItems.Any(x => x.Text == $"{sectorBaseName} {sectorNumber}"))
            {
                sectorNumber++;
                if (sectorNumber > 1000)
                    break;
            }

            _warehouseTreeItems.Add(new WarehouseTreeViewTempItem($"{sectorBaseName} {sectorNumber}", Enums.WarehouseStorageType.Sector));
        }

        private void AddRow(WarehouseTreeViewTempItem sector)
        {
            var rowBaseName = Localizer[Langs.Row];
            var rowNumber = (sector.Children?.Count ?? 0) + 1;
            while(sector.Children?.Any(x => x.Text == $"{rowBaseName} {rowNumber}") == true)
            {
                rowNumber++;
                if (rowNumber > 1000)
                    break;
            }
            
            sector.Children ??= new List<WarehouseTreeViewTempItem>();
            sector.AddChildren(new WarehouseTreeViewTempItem($"{rowBaseName} {rowNumber}", Enums.WarehouseStorageType.Row, sector.TempId));
            sector.Expanded = true;
        }

        private void AddSlot(WarehouseTreeViewTempItem row)
        {
            var slotBaseName = Localizer[Langs.Slot];
            var slotNumber = (row.Children?.Count ?? 0) + 1;
            while (row.Children?.Any(x => x.Text == $"{slotBaseName} {slotNumber}") == true)
            {
                slotNumber++;
                if (slotNumber > 1000)
                    break;
            }

            row.Children ??= new List<WarehouseTreeViewTempItem>();
            row.AddChildren(new WarehouseTreeViewTempItem($"{slotBaseName} {slotNumber}", Enums.WarehouseStorageType.Slot, row.TempId));
            row.Expanded = true;
        }


        private void RemoveStorage(WarehouseTreeViewTempItem? storage)
        {
            if (storage == null)
                return;

            if (storage.StorageType == Enums.WarehouseStorageType.Sector)
            {
                _warehouseTreeItems.Remove(storage);
            }
            else if (storage.StorageType == Enums.WarehouseStorageType.Row)
            {
                var parent = _warehouseTreeItems.FirstOrDefault(x => ((WarehouseTreeViewTempItem)x) != null && ((WarehouseTreeViewTempItem)x).TempId == storage.ParentId) as WarehouseTreeViewTempItem;
                if (parent == null)
                    return;
                parent.RemoveChildren(storage);
            }
            else if (storage.StorageType == Enums.WarehouseStorageType.Slot)
            {
                WarehouseTreeViewTempItem? parent = null;
                foreach (var sector in _warehouseTreeItems)
                {
                    parent = sector.Children?.FirstOrDefault(x => ((WarehouseTreeViewTempItem)x) != null && ((WarehouseTreeViewTempItem)x).TempId == storage.ParentId) as WarehouseTreeViewTempItem;
                    if (parent != null)
                        break;
                }
                if (parent == null)
                    return;
                parent.RemoveChildren(storage);
            }
        }

        private async Task DefineRows()
        {
            if (_isBusy || _loading)
                return;

            try
            {
                _isBusy = true;
                _loading = true;

                var sectors = _warehouseTreeItems.ToDictionary(x => ((WarehouseTreeViewTempItem)x).TempId, x => x.Text ?? "sector");
                if(sectors == null || sectors.Count == 0) 
                    return;

                var parameters = new DialogParameters<DefineRowsDialog<Guid>> { { x => x.Sectors, sectors } };
                var dialog = await DialogService.ShowAsync<DefineRowsDialog<Guid>>(Localizer[Langs.DefineRows], parameters);
                var confirmation = await dialog.Result;

                if (confirmation != null && !confirmation.Canceled)
                {
                    if (confirmation.Data is Tuple<IReadOnlyCollection<Guid>, int> data)
                    {
                        var sectorGuids = data.Item1.ToList();
                        var rowNumber = data.Item2;
                        if (rowNumber <= 0)
                            return;
                        
                        foreach(var sectorGuid in sectorGuids)
                        {
                            var sector = _warehouseTreeItems.FirstOrDefault(x => ((WarehouseTreeViewTempItem)x)?.TempId == sectorGuid) as WarehouseTreeViewTempItem;
                            if (sector == null)
                                continue;

                            sector.Children = new List<ITreeItemData<string>>();
                            for(int i=0; i<rowNumber;i++)
                                AddRow(sector);
                        }
                    }
                }
            }
            finally
            {
                _loading = false;
                _isBusy = false;
            }
        }

        private async Task DefineSlots(WarehouseTreeViewTempItem sector)
        {
            if (_isBusy || _loading)
                return;

            try
            {
                _isBusy = true;
                _loading = true;

                var rows = sector?.Children?.ToDictionary(x => ((WarehouseTreeViewTempItem)x).TempId, x => x.Text ?? "row");
                if (rows == null || rows.Count == 0)
                    return;

                var parameters = new DialogParameters<DefineSlotsDialog<Guid>> { { x => x.Rows, rows } };
                var dialog = await DialogService.ShowAsync<DefineSlotsDialog<Guid>>(Localizer[Langs.DefineSlots], parameters);
                var confirmation = await dialog.Result;

                if (confirmation != null && !confirmation.Canceled)
                {
                    if (confirmation.Data is Tuple<IReadOnlyCollection<Guid>, int> data)
                    {
                        var rowGuids = data.Item1.ToList();
                        var slotNumber = data.Item2;
                        if (slotNumber <= 0)
                            return;

                        foreach (var rowGuid in rowGuids)
                        {
                            var row = sector?.Children?.FirstOrDefault(x => ((WarehouseTreeViewTempItem)x)?.TempId == rowGuid) as WarehouseTreeViewTempItem;
                            if (row == null)
                                continue;

                            row.Children = new List<ITreeItemData<string>>();
                            for (int i = 0; i < slotNumber; i++)
                                AddSlot(row);
                        }
                    }
                }
            }
            finally
            {
                _loading = false;
                _isBusy = false;
            }
        }
    }
}
