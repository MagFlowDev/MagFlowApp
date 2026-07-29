using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Domain.CompanyScope
{
    public class Order : StatusEntity, ICodeEntity, IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; private set; }
        [Required]
        public int ContractorId { get; set; }
        [Required]
        public int OrderTypeId { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        public string? ClientOrderNumber { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public Guid? ConfirmedById { get; set; }
        public string? Note { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public Contractor? Contractor { get; set; }
        [ForeignKey(nameof(OrderTypeId))]
        public OrderType? OrderType { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
        [ForeignKey(nameof(ConfirmedById))]
        public User? ConfirmedBy { get; set; }

        public ICollection<OrderDelivery> Deliveries { get; set; }
        public ICollection<OrderDocument> Documents { get; set; }
        public ICollection<OrderItem> Items { get; set; }

       
        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown,
            Enums.EntityStatus.Active,
            Enums.EntityStatus.Deleted,
        };

        public Order() : base(_allowedStatuses) { }
    }
}
