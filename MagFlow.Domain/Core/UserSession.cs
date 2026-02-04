using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class UserSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string UserAgent { get; set; }
        [Required]
        public string IpAddress { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastTimeRecord { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        [Required]
        public string RefreshToken { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        public ICollection<SessionModule> SessionModules { get; set; }
    }
}
