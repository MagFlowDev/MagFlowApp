using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Enums.ProductStatus Status { get; set; }
        public ProductTypeDTO? Type { get; set; }
        public ProductCategoryDTO? Category { get; set; }
        public UnitDTO? Unit { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public Enums.TaxRate? TaxRate { get; set; }
        public Enums.Currency? Currency { get; set; }
        public DateTime? CreatedAt { get; set; }
        public UserDTO? CreatedBy { get; set; }

        public List<ParameterDTO> Parameters { get; set; } = new List<ParameterDTO>();
        public List<ComponentDTO> Components { get; set; } = new List<ComponentDTO>();
        public List<UnitConversionDTO> UnitConversions { get; set; } = new List<UnitConversionDTO>();
    }
}
