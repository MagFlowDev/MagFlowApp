using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.DTOs.Core
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Enums.NotificationType Type { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
