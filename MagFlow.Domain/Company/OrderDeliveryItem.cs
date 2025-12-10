using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class OrderDeliveryItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderDeliveryId { get; set; }
        [Required]
        public int OrderItemId { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        
        [ForeignKey(nameof(OrderDeliveryId))]
        public OrderDelivery? OrderDelivery { get; set; }
        [ForeignKey(nameof(OrderItemId))]
        public OrderItem? OrderItem { get; set; }
    }
}