using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class ProductUnitConversion : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int FromUnitId { get; set; }
        [Required]
        public int ToUnitId { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal ConversionRate { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [ForeignKey(nameof(FromUnitId))]
        public Unit? FromUnit { get; set; }
        [ForeignKey(nameof(ToUnitId))]
        public Unit? ToUnit { get; set; }
    }
}