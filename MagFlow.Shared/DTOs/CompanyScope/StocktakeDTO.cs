using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class StocktakeDTO : IBaseDTO, ICodeDTO
    {
        public int Id { get; set; }
        public string Code { get; init; } = string.Empty;
        public int TotalItems { get; set; }
        public int CheckedItems { get; set; }
        public decimal AgreementPercent => TotalItems == 0 ? 0m
            : decimal.Round(100m * CheckedItems / TotalItems, 1);
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public Enums.StocktakeType Type { get; set; }
        public Enums.EntityStatus Status { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
    }
}
