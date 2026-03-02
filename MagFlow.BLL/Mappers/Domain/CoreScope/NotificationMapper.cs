using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Domain.CoreScope
{
    public static class NotificationMapper
    {
        public static NotificationDTO ToDTO(this SystemNotification systemNotification)
        {
            return new NotificationDTO()
            {
                Id = systemNotification.Id,
                Title = systemNotification.Notification?.Title ?? "",
                Message = systemNotification.Notification?.Message ?? "",
                Type = Enums.NotificationType.System,
                ExpireAt = systemNotification.ExpireAt
            };
        }

        public static List<NotificationDTO> ToDTO(this ICollection<SystemNotification> systemNotifications)
        {
            return systemNotifications.Select(x => x.ToDTO()).ToList();
        }
    }
}
