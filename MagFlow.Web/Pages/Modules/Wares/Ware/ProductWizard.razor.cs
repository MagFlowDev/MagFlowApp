using MagFlow.BLL.Helpers;
using MagFlow.BLL.Helpers.Localization;
using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Web.Components.Wizards;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Wares.Ware
{
    public partial class ProductWizard
    {
        [Inject] public ILocalCacheService LocalCacheService { get; set; } = default!;
        [Inject] public IServiceProvider Services { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private string _typeSearchString = "";
        private string _categorySearchString = "";
        private string _unitSearchString = "";
        private int _pageSize = 25;

        private List<ProductTypeDTO> _productTypes = new List<ProductTypeDTO>();
        private List<ProductCategoryDTO> _productCategories = new List<ProductCategoryDTO>();
        private List<UnitDTO> _units = new List<UnitDTO>();

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
    }
}
