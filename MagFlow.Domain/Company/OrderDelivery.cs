using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace MagFlow.Domain.Company
{
    public class OrderDelivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int DocumentId { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public Document? Document { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public Contractor? Supplier { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public ICollection<OrderDeliveryItem> Items { get; set; }
    }
}