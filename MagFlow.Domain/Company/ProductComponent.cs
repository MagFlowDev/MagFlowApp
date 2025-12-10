using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class ProductComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int ComponentId { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        public string? Note { get; set; }
        [Required]
        public bool IsRequired { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [ForeignKey(nameof(ComponentId))]
        public Product? Component { get; set; }
    }
}