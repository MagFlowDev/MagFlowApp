using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;

namespace MagFlow.Domain.Company
{
    public class Process
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public string? Description { get; set; }
        [Required]
        public Enums.ProcessOriginType OriginType { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public Enums.ProcessStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? ClosedAt { get; set; }
        public Guid? ClosedById { get; set; }
        
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
        [ForeignKey(nameof(ClosedById))]
        public User? ClosedBy { get; set; }

        public ICollection<ProcessDocument> Documents { get; set; }
        public ICollection<ProcessStep> Steps { get; set; }
    }
}