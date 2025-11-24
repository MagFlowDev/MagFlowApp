using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class Product
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
        [Required]
        public int UnitId { get; set; }
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        [Precision(8,2)]
        public decimal? DefaultPurchasePrice { get; set; }
        [Precision(8,2)]
        public decimal? DefaultSellPrice { get; set; }
        [Precision(8,2)]
        public decimal? DefaultVatRate { get; set; }
        public Enums.Currency? Currency { get; set; }
        
        [ForeignKey(nameof(TypeId))]
        public ProductType? Type { get; set; }
        [ForeignKey(nameof(UnitId))]
        public Unit? Unit { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public ICollection<ProductComponent> Components { get; set; }
        public ICollection<ProductParameter> Parameters { get; set; }
        public ICollection<ProductUnitConversion> Conversions { get; set; }
    }
}