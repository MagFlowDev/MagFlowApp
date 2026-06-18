using MagFlow.BLL.Helpers;
using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Web.Components.Wizards;
using MagFlow.Web.Pages.Modules.Wares.Definition;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;

namespace MagFlow.Web.Pages.Modules.Wares.Ware
{
    public partial class ProductWizard : StepperWizardBase<ProductFormModel>
    {
        [Inject] public ILocalCacheService LocalCacheService { get; set; } = default!;
        [Inject] public IServiceProvider Services { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private string _typeSearchString = "";
        private string _categorySearchString = "";
        private string _unitSearchString = "";
        private string _parameterSearchString = "";
        private string _selectedParameterSearchString = "";
        private int _pageSize = 25;
        private bool _loadingAddParameter = false;

        private List<ProductTypeDTO> _productTypes = new List<ProductTypeDTO>();
        private List<ProductCategoryDTO> _productCategories = new List<ProductCategoryDTO>();
        private List<UnitDTO> _units = new List<UnitDTO>();
        private List<ParameterDTO> _parameters = new List<ParameterDTO>();

        private MudDropContainer<ParameterDTO> _container;

        protected override async Task OnInitializedAsync()
        {
            base.SetServices(LocalCacheService, Services, JS, Snackbar, NavigationManager);
            await base.OnInitializedAsync();

            _stepSections = new()
            {
                [0] = () => _model.GeneralInformation,
                [1] = () => _model.Parameters,
                [2] = () => _model.Prices,
            };
        }

        private void GenerateCode()
        {
            if (!string.IsNullOrEmpty(_model.GeneralInformation.Name))
                _model.GeneralInformation.Code = CodesGenerator.GeneratePrefix(_model.GeneralInformation.Name);
        }

        protected override async Task OnPreviewInteraction(StepperInteractionEventArgs arg)
        {
            await base.OnPreviewInteraction(arg);
            if (arg.StepIndex == 1 && arg.Action != StepAction.Complete)
                await SearchForProductParameter("", CancellationToken.None);
        }

        protected override async Task ControlStepCompletion(StepperInteractionEventArgs arg)
        {
            await base.ControlStepCompletion(arg);
            if(arg.StepIndex == 0)
                await SearchForProductParameter("", CancellationToken.None);
        }

        protected override async Task Save()
        {
            await base.Save();

            try
            {
                _isBusy = true;
                _loading = true;

                var result = await ProductService.AddProduct((ProductFormModel)_model);
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

        private async Task<IEnumerable<ProductTypeDTO>> SearchForProductType(string value, CancellationToken token)
        {
            _typeSearchString = value;
            var response = await ProductService.GetTypes(0, _pageSize, _typeSearchString);
            _productTypes = response.Elements;
            return _productTypes;
        }

        private async Task<IEnumerable<ProductCategoryDTO>> SearchForProductCategory(string value, CancellationToken token)
        {
            _categorySearchString = value;
            if(_model.GeneralInformation.ProductType == null)
            {
                _productCategories = new List<ProductCategoryDTO>();
                return _productCategories;
            }
            var response = await ProductService.GetCategories(0, _pageSize, _categorySearchString, productType: _model.GeneralInformation.ProductType);
            _productCategories = response.Elements;
            return _productCategories;
        }

        private async Task<IEnumerable<UnitDTO>> SearchForUnit(string value, CancellationToken token)
        {
            _unitSearchString = value;
            var response = await ProductService.GetUnits(0, _pageSize, _unitSearchString);
            _units = response.Elements;
            return _units;
        }

        private void ItemUpdated(MudItemDropInfo<ParameterDTO> dropItem)
        {
            if(dropItem.Item == null)
                return;
        
            dropItem.Item.DropZoneSelector = dropItem.DropzoneIdentifier;
        }

        private async Task OnParameterSearchChanged(string value)
        {
            _parameterSearchString = value;
            await SearchForProductParameter(_parameterSearchString, CancellationToken.None);
            StateHasChanged();
            _container?.Refresh();
        }

        private async Task OnSelectedParameterSearchChanged(string value)
        {
            _selectedParameterSearchString = value;
            var selectedParameters = _parameters.Where(x => x.DropZoneSelector == MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR).ToList();
            var searchedParameters = selectedParameters.Where(x => x.Name.IndexOf(_selectedParameterSearchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            var unsearchedParameters = selectedParameters.Except(searchedParameters).ToList();

            searchedParameters.ForEach(x => x.DropZoneHidden = false);
            unsearchedParameters.ForEach(x => x.DropZoneHidden = true);

            StateHasChanged();
            _container?.Refresh();
        }

        private async Task AddParameter()
        {
            if (_loading || _loadingAddParameter || _isBusy)
                return;

            try
            {
                _loadingAddParameter = true;

                if (!HasModulePermission("Wares", PermissionFlags.Add))
                    return;

                var dialog = await DialogService.ShowAsync<ParameterModal>(Localizer[Langs.AddParameter]);
                var confirmation = await dialog.Result;
                if (confirmation?.Data is bool result && result == true)
                {
                    await SearchForProductParameter(_parameterSearchString, CancellationToken.None);
                    StateHasChanged();
                    _container?.Refresh();
                }
            }
            finally
            {
                _loadingAddParameter = false;
            }
        }

        private void ChangeParemeterZone(ParameterDTO parameter, string zone)
        {
            parameter.DropZoneSelector = zone;
            parameter.DropZoneHidden = false;
            StateHasChanged();
            _container?.Refresh();
        }

        private async Task<IEnumerable<ParameterDTO>> SearchForProductParameter(string value, CancellationToken token)
        {
            _parameterSearchString = value;
            var response = await ProductService.GetParameters(0, _pageSize, _parameterSearchString);
            var alreadySelected = _parameters.Where(x => x.DropZoneSelector == MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR).ToList();
            var ids = alreadySelected.Select(x => x.Id);
            _parameters = response.Elements;
            _parameters = _parameters.Where(x => !ids.Contains(x.Id)).ToList();
            _parameters.AddRange(alreadySelected);
            return _parameters;
        }
    }
}
