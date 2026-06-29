using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class ItemFormModel
    {
        public ItemFormGeneralInformation GeneralInformation { get; set; }
        public ItemFormParameterValues ParameterValues { get; set; }
        
        public ItemFormModel()
        {
            GeneralInformation = new ItemFormGeneralInformation();
            ParameterValues = new ItemFormParameterValues();
        }
    }

    public class ItemFormGeneralInformation
    {
        public ProductDTO? Product { get; set; }
        public ProductTypeDTO? ProductType { get; set; }
        public ProductCategoryDTO? ProductCategory { get; set; }
        public string? Location { get; set; }
        public decimal? Quantity { get; set; }
        public UnitDTO? Unit { get; set; }
    }

    public class ItemFormParameterValues
    {
        public List<ItemFormParameterValue> Parameters { get; set; }

        public ItemFormParameterValues()
        {
            Parameters = new List<ItemFormParameterValue>();
        }
    }

    public class ItemFormParameterValue
    {
        public ParameterDTO Parameter { get; set; }
        public string Value { get; set; }

        public ItemFormParameterValue(ParameterDTO parameter, string value)
        {
            Parameter = parameter;
            Value = value;
        }
    }
}
