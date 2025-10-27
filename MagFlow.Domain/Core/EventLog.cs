using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class EventLog
    {
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Enums.EventLogLevel Level { get; set; }
        [Required]
        public Enums.EventLogCategory Category { get; set; }
        [Required]
        public string Message { get; set; }
        public string? Details { get; set; }
        [Required]
        public DateTime OccuredAt { get; set; }
        [Required]
        public string IpAddress { get; set; }
        [Required]
        public string UserAgent { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }
}
