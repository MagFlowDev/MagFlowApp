using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class StocktakeDTO : IBaseDTO
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public Enums.StocktakeType Type { get; set; }
        public Enums.EntityStatus Status { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
    }
}
