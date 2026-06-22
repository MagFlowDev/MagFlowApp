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

        protected override async Task OnInitializedAsync()
        {
            base.SetServices(LocalCacheService, Services, JS, Snackbar, NavigationManager);
            await base.OnInitializedAsync();

            _stepSections = new()
            {
                [0] = () => _model.GeneralInformation,
                [1] = () => _model.ParameterValues
            };
        }

        protected override async Task Save()
        {
            await base.Save();

            try
            {
                _isBusy = true;
                _loading = true;

                //var result = await ProductService.AddProduct((ProductFormModel)_model);
                //if (result == Enums.Result.Success)
                //{
                //    NavigationManager.NavigateTo("/");
                //    Snackbar.Add(Localizer[Langs.ActionSucceed], Severity.Success);
                //    return;
                //}
                //else
                //{
                //    Snackbar.Add(Localizer[Langs.ErrorOccured], Severity.Error);
                //}
            }
            finally
            {
                _isBusy = false;
                _loading = false;
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
