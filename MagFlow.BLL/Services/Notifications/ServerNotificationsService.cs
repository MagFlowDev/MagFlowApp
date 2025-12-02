using MagFlow.BLL.Hubs;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.Company;
using MagFlow.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Notifications
{
    public class ServerNotificationsService : IServerNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public ServerNotificationsService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifyAllAsync(string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null)
        {
            var payload = new { Title = title, Message = message, Timestamp = DateTime.UtcNow, Type = type, ExpireAt = ExpireAt };
            var serialized = JsonSerializer.Serialize(payload);
            return _hubContext.Clients.All.SendAsync("ReceiveNotification", serialized);
        }

        public Task NotifyUserAsync(string userId, string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null)
        {
            var payload = new { Title = title, Message = message, Timestamp = DateTime.UtcNow, Type = type, ExpireAt = ExpireAt };
            return _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", payload);
        }

        public Task NotifyUsersAsync(List<string> userIds, string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null)
        {
            var payload = new { Title = title, Message = message, Timestamp = DateTime.UtcNow, Type = type, ExpireAt = ExpireAt };
            return _hubContext.Clients.Users(userIds).SendAsync("ReceiveNotification", payload);
        }
    }
}
