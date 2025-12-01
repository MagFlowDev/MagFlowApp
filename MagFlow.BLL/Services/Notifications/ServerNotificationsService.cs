using MagFlow.BLL.Hubs;
using MagFlow.BLL.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MagFlow.Shared.Models;

namespace MagFlow.BLL.Services.Notifications
{
    public class ServerNotificationsService : IServerNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public ServerNotificationsService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifyAllAsync(string message)
        {
            var payload = new { Message = message, Timestamp = DateTime.UtcNow, Type = Enums.NotificationType.System };
            var serialized = JsonSerializer.Serialize(payload);
            return _hubContext.Clients.All.SendAsync("ReceiveNotification", serialized);
        }

        public Task NotifyUserAsync(string userId, string message)
        {
            var payload = new { Message = message, Timestamp = DateTime.UtcNow, Type = Enums.NotificationType.User };
            return _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", payload);
        }
    }
}
