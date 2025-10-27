using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class UserNotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid NotificationId { get; set; }
        [Required]
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        [Required]
        public DateTime DeliveredAt { get; set; }
        [Required]
        public bool IsArchived { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
        [ForeignKey(nameof(NotificationId))]
        public Notification? Notification { get; set; }
    }
}
