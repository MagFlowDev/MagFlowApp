using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class StocktakeItemDTO
    {
        public int Id { get; set; }
        public Enums.EntityStatus ProductStatus { get; set; }
        public decimal OriginalQuantity { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public Enums.StocktakeItemCheckStatus CheckStatus { get; set; }
        public StocktakeItemValues Values { get; set; } = new();
        public List<StocktakeParameterDTO> Parameters { get; set; } = [];
    }

}
