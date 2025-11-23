using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class ProductUnitConversion
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
        [Precision(8,2)]
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