using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Domain
{
    public static class CoreDTOMapper
    {
        public static UserDTO ToDTO(this ApplicationUser applicationUser)
        {
            return new UserDTO()
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Settings = ToDTO(applicationUser.UserSettings)
            };
        }

        public static UserSettingsDTO ToDTO(this ApplicationUserSettings? applicationUserSettings)
        {
            return new UserSettingsDTO()
            {
                Language = applicationUserSettings?.Language ?? Shared.Models.Enums.Language.Polish,
            };
        }

        public static NotificationDTO ToDTO(this SystemNotification systemNotification)
        {
            return new NotificationDTO()
            {
                Id = systemNotification.Id,
                Title = systemNotification.Notification?.Title ?? "",
                Message = systemNotification.Notification?.Message ?? ""
            };
        }

        public static List<NotificationDTO> ToDTO(this ICollection<SystemNotification> systemNotifications)
        {
            return systemNotifications.Select(x => x.ToDTO()).ToList();
        }
    }
}
