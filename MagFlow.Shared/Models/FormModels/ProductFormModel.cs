using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.DTOs.CompanyScope;

namespace MagFlow.Shared.Models.FormModels
{
    public class ProductFormModel
    {
        public ProductFormGeneralInformation GeneralInformation { get; set; }
        public ProductFormParameters Parameters { get; set; }
        public ProductFormPrices Prices { get; set; }

        public ProductFormModel()
        {
            GeneralInformation = new ProductFormGeneralInformation();
            Parameters = new ProductFormParameters();
            Prices = new ProductFormPrices();
        }
    }

    public class ProductFormGeneralInformation
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public ProductTypeDTO? ProductType { get; set; }
        public ProductCategoryDTO? ProductCategory { get; set; }
        public UnitDTO? Unit { get; set; }
    }

    public class ProductFormParameters
    {
        public List<ParameterDTO> Parameters { get; set; }

        public ProductFormParameters()
        {
            Parameters = new List<ParameterDTO>();
        }
    }

    public class ProductFormPrices
    {
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public Enums.TaxRate? TaxRate { get; set; }
        public Enums.Currency? Currency { get; set; }
    }

    public class ProductTypeFormModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ProductCategoryDTO? ProductCategory { get; set; }
    }

    public class ProductCategoryFormModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsBasic { get; set; }

        public ProductCategoryFormModel()
        {
            IsBasic = true;
        }
    }

    public class ProductParameterFormModel
    {
        public string Name { get; set; }
    }

    public class ParameterFormModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Enums.ValueType? ValueType { get; set; }
        public UnitDTO? Unit { get; set; }
    }

}
