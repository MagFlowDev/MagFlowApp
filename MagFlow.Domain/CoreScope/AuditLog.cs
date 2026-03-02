using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MagFlow.Domain.CoreScope
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string EntityName { get; set; }
        [Required]
        public Guid RelatedEntityId { get; set; }
        [Required]
        public Enums.AuditLogAction Action { get; set; }
        [Required]
        public DateTime OccuredAt { get; set; }
        [Required]
        public string IpAddress { get; set; }
        [Required]
        public string UserAgent { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        public ICollection<AuditLogChange> Changes { get; set; }
    }
}
