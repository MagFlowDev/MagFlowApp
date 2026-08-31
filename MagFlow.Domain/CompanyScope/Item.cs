using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Domain.CompanyScope;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class Item : StatusEntity, IBaseEntity, ICodeEntity, ISoftDeletable, IHistoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
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
        public DateTime? RemovedAt { get; set; }
        public Guid? RemovedById { get; set; }
        public Enums.RemovalReason? RemovalReason { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ConsumptionDate { get; set; }
        [Required]
        public Enums.Condition Condition { get; set; }
        public string? Note { get; set; }
        [Precision(18, 4)]
        public decimal? PurchasePrice { get; set; }
        [Precision(18, 4)]
        public decimal? SellPrice { get; set; }
        [Precision(18, 4)]
        public decimal? TaxRate { get; set; }
        [Required]
        public int DefaultUnitId { get; set; }
        public Enums.Currency? Currency { get; set; }
        
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
        [ForeignKey(nameof(RemovedById))]
        public User? RemovedBy { get; set; }
        [ForeignKey(nameof(DefaultUnitId))]
        public Unit? DefaultUnit { get; set; }

        public ICollection<ItemParameter> Parameters { get; set; } = [];
        public ICollection<ItemComponent> Components { get; set; } = [];

        [NotMapped]
        public override Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.Item;


        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown, 
            Enums.EntityStatus.Available, 
            Enums.EntityStatus.Blocked, 
            Enums.EntityStatus.Deleted, 
            Enums.EntityStatus.Used,
            Enums.EntityStatus.Released
        };

        public Item() : base(_allowedStatuses) { }
    }
}