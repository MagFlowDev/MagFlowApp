using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId {get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Precision(8,2)]
        public decimal Quantity { get; set; }
        [Required]
        [Precision(8,2)]
        public decimal Price { get; set; }
        [Required]
        [Precision(8,2)]
        public decimal VatRate { get; set; }
        [Required]
        public Enums.Currency Currency { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}