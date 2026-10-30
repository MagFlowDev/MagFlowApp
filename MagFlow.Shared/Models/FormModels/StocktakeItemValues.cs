using MagFlow.Shared.Models;

namespace MagFlow.Shared.Models.FormModels
{
    public class StocktakeItemValues
    {
        public decimal Quantity { get; set; }
        public string? ExternalId { get; set; }
        public string? Location { get; set; }
        public int? WarehouseId { get; set; }
        public int? SectorId { get; set; }
        public int? RowId { get; set; }
        public int? SlotId { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ConsumptionDate { get; set; }
        public Enums.Condition Condition { get; set; }
        // Keys identify the copied parameter rows, not shared definitions.
        public Dictionary<int, string> Parameters { get; set; } = [];
    }
}
