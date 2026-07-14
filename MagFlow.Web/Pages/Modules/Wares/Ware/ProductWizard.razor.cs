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
using System.Text.Json;

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
        private string _productSearchString = "";
        private string _selectedComponentSearchString = "";
        private int _pageSize = 25;
        private bool _loadingAddParameter = false;

        private List<ProductTypeDTO> _productTypes = new List<ProductTypeDTO>();
        private List<ProductCategoryDTO> _productCategories = new List<ProductCategoryDTO>();
        private List<UnitDTO> _units = new List<UnitDTO>();
        private List<ParameterDTO> _parameters = new List<ParameterDTO>();
        private List<ComponentDTO> _components = new List<ComponentDTO>();

        private MudDropContainer<ParameterDTO> _parametersContainer;
        private MudDropContainer<ComponentDTO> _componentsContainer;

        private string _stepperKey => $"{ShowComponentsStep}";

        private bool ShowComponentsStep => _model.GeneralInformation.ProductCategory != null
            && _model.GeneralInformation.ProductCategory.IsBasic == false;

        protected override async Task OnInitializedAsync()
        {
            base.SetServices(LocalCacheService, Services, JS, Snackbar, NavigationManager);
            await base.OnInitializedAsync();

            _stepSections = new()
            {
                [0] = () => _model.GeneralInformation,
                [1] = () => _model.Parameters,
                [2] = () => _model.Components,
                [3] = () => _model.Prices,
            };

            try
            {
                var copiedItem = await LocalCacheService.PasteItem();
                var data = copiedItem.Item1;
                var dataType = copiedItem.Item2;
                if(data != null && dataType == typeof(ProductDTO).Name && data is JsonElement element)
                {
                    var product = element.Deserialize<ProductDTO>();
                    if(product != null)
                        CreateCopy(product);
                }
            }
            catch { }
        }

        private void CreateCopy(ProductDTO dto)
        {
            _model.GeneralInformation.Name = dto.Name;
            _model.GeneralInformation.Code = dto.Code;
            _model.GeneralInformation.ProductCategory = dto.Category;
            _model.GeneralInformation.ProductType = dto.Type;
            _model.GeneralInformation.Unit = dto.Unit;

            dto.Parameters?.ForEach(parameter =>
            {
                _model.Parameters.Parameters.Add(parameter);
                parameter.DropZoneSelector = MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR;
                _parameters.Add(parameter);
            });

            dto.Components?.ForEach(component =>
            {
                _model.Components.Components.Add(component);
                component.DropZoneSelector = MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR;
                _components.Add(component);
            });

            _model.Prices.PurchasePrice = dto.PurchasePrice;
            _model.Prices.SellingPrice = dto.SellingPrice;
            _model.Prices.TaxRate = dto.TaxRate;
            _model.Prices.Currency = dto.Currency;
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
            else if (arg.StepIndex == 2 && arg.Action != StepAction.Complete)
                await SearchForProductComponent("", CancellationToken.None);
        }

        protected override async Task ControlStepCompletion(StepperInteractionEventArgs arg)
        {
            await base.ControlStepCompletion(arg);
            if(arg.StepIndex == 0)
            {
                await SearchForProductParameter("", CancellationToken.None);
                await SearchForProductComponent("", CancellationToken.None);
            }
        }

        protected override async Task Save()
        {
            _model.Parameters.Parameters = _parameters.Where(x => x.DropZoneSelector == MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR).ToList();
            _model.Components.Components = _components.Where(x => x.DropZoneSelector == MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR && x.Quantity > 0).ToList();
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

        private void OnProductCategoryChanged(ProductCategoryDTO category)
        {
            if (_model.GeneralInformation.ProductCategory != category)
                _model.GeneralInformation.ProductType = null;
            _model.GeneralInformation.ProductCategory = category;
            if(category == null || category.IsBasic)
            {
                _stepSections = new()
                {
                    [0] = () => _model.GeneralInformation,
                    [1] = () => _model.Parameters,
                    [2] = () => _model.Prices,
                };
            }
            else
            {
                _stepSections = new()
                {
                    [0] = () => _model.GeneralInformation,
                    [1] = () => _model.Parameters,
                    [2] = () => _model.Components,
                    [3] = () => _model.Prices,
                };
            }
            StateHasChanged();
        }

        private async Task<IEnumerable<ProductTypeDTO>> SearchForProductType(string value, CancellationToken token)
        {
            _typeSearchString = value;
            if (_model.GeneralInformation.ProductCategory == null)
            {
                _productTypes = new List<ProductTypeDTO>();
                return _productTypes;
            }
            var response = await ProductService.GetTypes(0, _pageSize, _typeSearchString, productCategory: _model.GeneralInformation.ProductCategory);
            _productTypes = response.Elements;
            return _productTypes;
        }

        private async Task<IEnumerable<ProductCategoryDTO>> SearchForProductCategory(string value, CancellationToken token)
        {
            _categorySearchString = value;
            var response = await ProductService.GetCategories(0, _pageSize, _categorySearchString);
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

        private void ItemUpdated(MudItemDropInfo<ComponentDTO> dropItem)
        {
            if (dropItem.Item == null)
                return;

            dropItem.Item.DropZoneSelector = dropItem.DropzoneIdentifier;
        }

        private async Task OnParameterSearchChanged(string value)
        {
            _parameterSearchString = value;
            await SearchForProductParameter(_parameterSearchString, CancellationToken.None);
            StateHasChanged();
            _parametersContainer?.Refresh();
        }

        private async Task OnProductSearchChanged(string value)
        {
            _productSearchString = value;
            await SearchForProductComponent(_productSearchString, CancellationToken.None);
            StateHasChanged();
            _componentsContainer?.Refresh();
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
            _parametersContainer?.Refresh();
        }

        private async Task OnSelectedProductSearchChanged(string value)
        {
            _selectedComponentSearchString = value;
            var selectedComponents = _components.Where(x => x.DropZoneSelector == MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR).ToList();
            var searchedComponents = selectedComponents.Where(x => x.Product.Name.IndexOf(_selectedComponentSearchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            var unsearchedComponents = selectedComponents.Except(searchedComponents).ToList();

            searchedComponents.ForEach(x => x.DropZoneHidden = false);
            unsearchedComponents.ForEach(x => x.DropZoneHidden = true);

            StateHasChanged();
            _componentsContainer?.Refresh();
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
                    _parametersContainer?.Refresh();
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
            _parametersContainer?.Refresh();
        }

        private async Task<IEnumerable<ParameterDTO>> SearchForProductParameter(string value, CancellationToken token)
        {
            _parameterSearchString = value;
            var response = await ProductService.GetParameters(0, _pageSize, _parameterSearchString);
            var alreadySelected = _parameters.Where(x => x.DropZoneSelector == MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR).ToList();
            var ids = alreadySelected.Select(x => x.ParameterId);
            _parameters = response.Elements;
            _parameters = _parameters.Where(x => !ids.Contains(x.ParameterId)).ToList();
            _parameters.AddRange(alreadySelected);
            return _parameters;
        }

        private void ChangeComponentZone(ComponentDTO component, string zone)
        {
            component.DropZoneSelector = zone;
            component.DropZoneHidden = false;
            StateHasChanged();
            _componentsContainer?.Refresh();
        }

        private async Task<IEnumerable<ComponentDTO>> SearchForProductComponent(string value, CancellationToken token)
        {
            _productSearchString = value;
            var response = await ProductService.GetProducts(0, _pageSize, _productSearchString);
            var alreadySelected = _components.Where(x => x.DropZoneSelector == MagFlow.Shared.Constants.Identificators.DropZoneID.SELECTED_SELECTOR).ToList();
            var ids = alreadySelected.Select(x => x.Product.Id);
            var products = response.Elements;
            var components = products.Select(x => new ComponentDTO() { Product = x, Quantity = 1, IsRequired = true });
            _components = components.Where(x => !ids.Contains(x.Product.Id)).ToList();
            _components.AddRange(alreadySelected);
            return _components;
        }
    }
}
