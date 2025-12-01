using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Domain.Core
{
    public class CompanyNotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public Guid NotificationId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ExpireAt { get; set; }
        [Required]
        public bool IsArchived { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
        [ForeignKey(nameof(NotificationId))]
        public Notification? Notification { get; set; }
    }
}
