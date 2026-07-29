using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
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
            _model.GeneralInformation.Code = dto.Code;

            _model.Sectors = new List<WarehouseFormSector>();
            dto.Sectors?.ForEach(sector =>
            {
                var newSector = new WarehouseFormSector();
                newSector.Name = sector.Name;
                newSector.Code = sector.Code;
                sector.Rows?.ForEach(row =>
                {
                    var newRow = new WarehouseFormSectorRow();
                    newRow.Name = row.Name;
                    newRow.Code = row.Code;
                    row.Slots?.ForEach(slot =>
                    {
                        var newSlot = new WarehouseFormSectorRowSlot();
                        newSlot.Name = slot.Name;
                        newSlot.Code = slot.Code;
                        newRow.Slots.Add(newSlot);
                    });
                    newSector.Rows.Add(newRow);
                });
                _model.Sectors.Add(newSector);
            });
        }

        protected override async Task Save()
        {
            if (_isBusy)
                return;

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
    }
}
