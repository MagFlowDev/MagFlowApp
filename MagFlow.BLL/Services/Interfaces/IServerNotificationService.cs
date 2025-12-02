using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IServerNotificationService
    {
        Task NotifyAllAsync(string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null);
        Task NotifyUserAsync(string userId, string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null);
        Task NotifyUsersAsync(List<string> userIds, string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null);
    }
}
