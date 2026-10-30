using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Domain.CompanyScope;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CompanyScope
{
    public class StocktakeItem : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int StocktakeId { get; set; }
        public Enums.StocktakeItemCheckStatus CheckStatus { get; set; }
        // Captured before the first correction. Existing copies also retain their baseline.
        public string? OriginalValuesJson { get; set; }
        public string Code { get; private set; }
        public string? ExternalId { get; set; }
        [Required]
        public int ProductId { get; set; }


        // Location
        public int? WarehouseId { get; set; }
        public int? SectorId { get; set; }
        public int? RowId { get; set; }
        public int? SlotId { get; set; }
        public string? Location { get; set; }

        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ConsumptionDate { get; set; }
        [Required]
        public Enums.Condition Condition { get; set; }
        [Required]
        public int DefaultUnitId { get; set; }

        [ForeignKey(nameof(StocktakeId))]
        public Stocktake? Stocktake { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }
        [ForeignKey(nameof(SectorId))]
        public WarehouseSector? Sector { get; set; }
        [ForeignKey(nameof(RowId))]
        public WarehouseSectorRow? Row { get; set; }
        [ForeignKey(nameof(SlotId))]
        public WarehouseSectorRowSlot? Slot { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
        [ForeignKey(nameof(DefaultUnitId))]
        public Unit? DefaultUnit { get; set; }

        public ICollection<StocktakeItemParameter> Parameters { get; set; } = [];


        public MagFlow.Shared.Models.FormModels.StocktakeItemValues GetValues()
        {
            return new MagFlow.Shared.Models.FormModels.StocktakeItemValues
            {
                Quantity = Quantity, ExternalId = ExternalId, Location = Location,
                WarehouseId = WarehouseId, SectorId = SectorId, RowId = RowId, SlotId = SlotId,
                ReceivedAt = ReceivedAt, ProductionDate = ProductionDate,
                ConsumptionDate = ConsumptionDate, Condition = Condition,
                Parameters = Parameters.OrderBy(x => x.Id).ToDictionary(x => x.Id, x => x.Value)
            };
        }

        public string SerializeValues() => System.Text.Json.JsonSerializer.Serialize(GetValues());

        [NotMapped]
        public bool HasCorrections => OriginalValuesJson != null && OriginalValuesJson != SerializeValues();

        public static StocktakeItem CreateItemCopy(Item item, int stocktakeId = 0)
        {
            return new StocktakeItem()
            {
                StocktakeId = stocktakeId,
                Code = item.Code,
                ExternalId = item.ExternalId,
                ProductId = item.ProductId,
                WarehouseId = item.WarehouseId,
                SectorId = item.SectorId,
                RowId = item.RowId,
                SlotId = item.SlotId,
                Location = item.Location,
                Quantity = item.Quantity,
                CreatedAt = item.CreatedAt,
                CreatedById = item.CreatedById,
                ReceivedAt = item.ReceivedAt,
                ProductionDate = item.ProductionDate,
                ConsumptionDate = item.ConsumptionDate,
                Condition = item.Condition,
                DefaultUnitId = item.DefaultUnitId,
                Parameters = item.Parameters.Select(p => StocktakeItemParameter.CreateParameterCopy(p)).ToList(),
            };
        }
    }
}
