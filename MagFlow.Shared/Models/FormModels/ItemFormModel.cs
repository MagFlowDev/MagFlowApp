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
        public ItemFormComponents Components { get; set; }
        
        public ItemFormModel()
        {
            GeneralInformation = new ItemFormGeneralInformation();
            ParameterValues = new ItemFormParameterValues();
            Components = new ItemFormComponents();
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

    public class ItemFormComponents
    {
        public List<ItemFormComponent> Components { get; set; }

        public ItemFormComponents()
        {
            Components = new List<ItemFormComponent>();
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

    public class ItemFormComponent
    {
        public ProductDTO Product { get; set; }
        public decimal RequiredQuantity { get; set; }

        public List<ItemFormComponentRecord> Components { get; set; }

        public ItemFormComponent(ProductDTO product, decimal requiredQuantity)
        {
            Product = product;
            RequiredQuantity = requiredQuantity;
            Components = new List<ItemFormComponentRecord>();
        }
    }

    public class ItemFormComponentRecord
    {
        public ItemDTO Item { get; set; }
        public decimal Quantity { get; set; }
    }
}
