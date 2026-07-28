using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class OrderItem : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId {get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Price { get; set; }
        [Required]
        [Precision(18, 4)]
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