using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class Product : ISoftDeletable, IHistoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int TypeId { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        public int UnitId { get; set; }
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        [Precision(18, 4)]
        public decimal? DefaultPurchasePrice { get; set; }
        [Precision(18, 4)]
        public decimal? DefaultSellPrice { get; set; }
        [Precision(18, 4)]
        public decimal? DefaultVatRate { get; set; }
        public Enums.Currency? Currency { get; set; }
        
        [ForeignKey(nameof(TypeId))]
        public ProductType? Type { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public ProductCategory? Category { get; set; }
        [ForeignKey(nameof(UnitId))]
        public Unit? Unit { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public ICollection<ProductComponent> Components { get; set; }
        public ICollection<ProductParameter> Parameters { get; set; }
        public ICollection<ProductUnitConversion> Conversions { get; set; }

        public DateTime? RemovedAt { get; set; }

        [NotMapped]
        public Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.Product;

        [NotMapped]
        public ICollection<IEntityHistory> History { get; set; } = [];
    }
}