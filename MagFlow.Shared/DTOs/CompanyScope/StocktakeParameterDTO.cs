using MagFlow.Shared.Models;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class StocktakeParameterDTO
    {
        public int Id { get; set; }
        public string OriginalValue { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public Enums.ValueType ValueType { get; set; }
    }
}
