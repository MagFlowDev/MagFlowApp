using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int WarehouseId { get; set; }
        public int? StorageId { get; set; }
        public string? Location { get; set; }
        [Required]
        [Precision(8,2)]
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
        public bool IsBlocked { get; set; }
        [Required]
        public Enums.Condition Condition { get; set; }
        [Required]
        public Enums.ItemStatus Status { get; set; }
        public string? Note { get; set; }
        [Precision(8,2)]
        public decimal? PurchasePrice { get; set; }
        [Precision(8,2)]
        public decimal? SellPrice { get; set; }
        [Precision(8,2)]
        public decimal? VatRate { get; set; }
        public Enums.Currency? Currency { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }
        [ForeignKey(nameof(StorageId))]
        public WarehouseStorage? Storage { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
        [ForeignKey(nameof(RemovedById))]
        public User? RemovedBy { get; set; }

        public ICollection<ItemParameter> Parameters { get; set; }
    }
}