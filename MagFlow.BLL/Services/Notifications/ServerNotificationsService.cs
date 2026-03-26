using MagFlow.BLL.Hubs;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ServerNotificationsService> _logger;

        public ServerNotificationsService(IHubContext<NotificationHub> hubContext,
            ILogger<ServerNotificationsService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyAllAsync(string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null)
        {
            var payload = new { Title = title, Message = message, Timestamp = DateTime.UtcNow, Type = type, ExpireAt = ExpireAt };
            var serialized = JsonSerializer.Serialize(payload);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", serialized);
        }

        public async Task NotifyUserAsync(string userId, string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null)
        {
            var payload = new { Title = title, Message = message, Timestamp = DateTime.UtcNow, Type = type, ExpireAt = ExpireAt };
            var connectionId = NotificationHub.GetConnectionId(userId);
            if(!string.IsNullOrEmpty(connectionId))
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", payload);
        }

        public async Task NotifyUsersAsync(List<string> userIds, string title, string message, Enums.NotificationType type, DateTime? ExpireAt = null)
        {
            var payload = new { Title = title, Message = message, Timestamp = DateTime.UtcNow, Type = type, ExpireAt = ExpireAt };
            List<string> connectionIds = new List<string>();
            foreach (var userId in userIds)
            {
                var connectionId = NotificationHub.GetConnectionId(userId);
                if (!string.IsNullOrEmpty(connectionId))
                    connectionIds.Add(connectionId);
            }
            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveNotification", payload);
        }

        public async Task ForceUserLogoutAsync(string userId)
        {
            var connectionId = NotificationHub.GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
                await _hubContext.Clients.Client(connectionId).SendAsync("ForceLogout");
        }

        public async Task ForceUserLogoutAsync(List<string> userId)
        {
            var connectionIds = userId.Select(x => NotificationHub.GetConnectionId(x)).Where(x => x != null);
            if(connectionIds.Any())
                await _hubContext.Clients.Clients(connectionIds).SendAsync("ForceLogout");
        }
    }
}
