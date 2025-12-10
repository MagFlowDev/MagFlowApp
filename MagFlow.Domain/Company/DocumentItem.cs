using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class DocumentItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int DocumentHeaderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int WarehouseId { get; set; }
        public int? StorageId { get; set; }
        public string? Location { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        [Required]
        public Enums.Condition Condition { get; set; }
        public string? Note { get; set; }
        [Precision(18, 4)]
        public decimal? PurchasePrice { get; set; }
        [Precision(18, 4)]
        public decimal? SellPrice { get; set; }
        [Precision(18, 4)]
        public decimal? VatRate { get; set; }
        public Enums.Currency? Currency { get; set; }
        
        [ForeignKey(nameof(DocumentHeaderId))]
        public Document? DocumentHeader { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }
        [ForeignKey(nameof(StorageId))]
        public WarehouseStorage? Storage { get; set; }
    }
}