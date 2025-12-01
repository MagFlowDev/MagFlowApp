using MagFlow.BLL.Hubs;
using MagFlow.BLL.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task NotifyAllAsync(string message)
        {
            var payload = new { Message = message, Timestamp = DateTime.UtcNow };
            return _hubContext.Clients.All.SendAsync("ReceiveNotification", payload);
        }

        public Task NotifyUserAsync(string userId, string message)
        {
            var payload = new { Message = message, Timestamp = DateTime.UtcNow };
            return _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", payload);
        }
    }
}
