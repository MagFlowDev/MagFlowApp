using MagFlow.Shared.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetCurrentSystemNotificationsAsync();
    }
}
