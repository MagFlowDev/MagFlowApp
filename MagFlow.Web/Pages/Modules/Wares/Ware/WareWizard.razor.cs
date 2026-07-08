using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Web.Components.Wizards;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace MagFlow.Web.Pages.Modules.Wares.Ware
{
    public partial class WareWizard : StepperWizardBase<ItemFormModel>
    {
        [Inject] public ILocalCacheService LocalCacheService { get; set; } = default!;
        [Inject] public IServiceProvider Services { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private string _productSearchString = "";
        private int _pageSize = 25;

        private List<ProductDTO> _products = new List<ProductDTO>();

        private string _stepperKey => $"{ShowComponentsStep}";

        private bool ShowComponentsStep => _model.Components?.Components != null
            && _model.Components.Components.Any(x => x.Product != null);

        private AddComponentType _addComponentType { get; set; } = AddComponentType.ReadyProduct;

        protected override async Task OnInitializedAsync()
        {
            base.SetServices(LocalCacheService, Services, JS, Snackbar, NavigationManager);
            await base.OnInitializedAsync();

            _stepSections = new()
            {
                [0] = () => _model.GeneralInformation,
                [1] = () => _model.ParameterValues
            };

            try
            {
                var copiedItem = await LocalCacheService.PasteItem();
                var data = copiedItem.Item1;
                var dataType = copiedItem.Item2;
                if (data != null && dataType == typeof(ItemDTO).Name && data is JsonElement element)
                {
                    var product = element.Deserialize<ItemDTO>();
                    if (product != null)
                        CreateCopy(product);
                }
            }
            catch { }
        }

        private void CreateCopy(ItemDTO dto)
        {
            _model.GeneralInformation.Product = dto.Product;
            _model.GeneralInformation.ProductType = dto.Product?.Type;
            _model.GeneralInformation.ProductCategory = dto.Product?.Category;
            _model.GeneralInformation.Location = dto.Location;
            _model.GeneralInformation.Quantity = dto.Quantity;
            _model.GeneralInformation.Unit = dto.Unit;

            _model.ParameterValues.Parameters = new List<ItemFormParameterValue>();
            dto.Parameters?.ForEach(parameter =>
            {
                _model.ParameterValues.Parameters.Add(new ItemFormParameterValue(parameter.Parameter, parameter.Value));
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

                var result = await ItemService.AddItem((ItemFormModel)_model);
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

        enum AddComponentType
        {
            ReadyProduct,
            AddComponents
        }

        private void ProductSelected(ProductDTO product)
        {
            _model.GeneralInformation.Product = product;
            _model.GeneralInformation.ProductCategory = product.Category;
            _model.GeneralInformation.ProductType = product.Type;
            _model.GeneralInformation.Unit = product.Unit;

            _model.ParameterValues.Parameters = new List<ItemFormParameterValue>();
            foreach(var parameter in product.Parameters)
            {
                _model.ParameterValues.Parameters.Add(new ItemFormParameterValue(parameter, null));
            }
            foreach(var component in product.Components)
            {
                _model.Components.Components.Add(new ItemFormComponent(component.Product, component.Quantity));
            }
        }

        private void OnParameterValueChanged(ParameterDTO parameter, object value)
        {
            var parameterValue = _model.ParameterValues.Parameters.FirstOrDefault(p => p.Parameter == parameter);
            if (parameterValue != null)
            {
                parameterValue.Value = value?.ToString();
            }
        }

        private async Task<IEnumerable<ProductDTO>> SearchForProduct(string value, CancellationToken token)
        {
            _productSearchString = value;
            var response = await ProductService.GetProducts(0, _pageSize, _productSearchString);
            _products = response.Elements;
            return _products;
        }
    }
}
