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
    public class StockMovement : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        [Required]
        public Enums.StockMovementType MovementType { get; set; }
        public int? DocumentId { get; set; }

        public int? SourceWarehouseId { get; set; }
        public int? SourceSectorId { get; set; }
        public int? SourceRowId { get; set; }
        public int? SourceSlotId { get; set; }

        public int? TargetWarehouseId { get; set; }
        public int? TargetSectorId { get; set; }
        public int? TargetRowId { get; set; }
        public int? TargetSlotId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }


        [ForeignKey(nameof(ItemId))]
        public Item? Item { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public Document? Document { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        [ForeignKey(nameof(SourceWarehouseId))]
        public Warehouse? SourceWarehouse { get; set; }
        [ForeignKey(nameof(SourceSectorId))]
        public WarehouseSector? SourceSector { get; set; }
        [ForeignKey(nameof(SourceRowId))]
        public WarehouseSectorRow? SourceRow { get; set; }
        [ForeignKey(nameof(SourceSlotId))]
        public WarehouseSectorRowSlot? SourceSlot { get; set; }

        [ForeignKey(nameof(TargetWarehouseId))]
        public Warehouse? TargetWarehouse { get; set; }
        [ForeignKey(nameof(TargetSectorId))]
        public WarehouseSector? TargetSector { get; set; }
        [ForeignKey(nameof(TargetRowId))]
        public WarehouseSectorRow? TargetRow { get; set; }
        [ForeignKey(nameof(TargetSlotId))]
        public WarehouseSectorRowSlot? TargetSlot { get; set; }
    }
}
