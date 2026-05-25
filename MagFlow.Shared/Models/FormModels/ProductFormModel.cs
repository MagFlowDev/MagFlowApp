using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.DTOs.CompanyScope;

namespace MagFlow.Shared.Models.FormModels
{
    public class ProductFormModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
       

    }

    public class ProductTypeFormModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsBasic { get; set; }
    }

    public class ProductCategoryFormModel
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public ProductTypeDTO? ProductType { get; set; }
    }

    public class ProductParameterFormModel
    {
        public string Name { get; set; }
    }

    public class ParameterFormModel
    {
        public string Name { get; set; }
    }

}
