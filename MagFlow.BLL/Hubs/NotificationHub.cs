using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MagFlow.BLL.Hubs
{
    public class NotificationHub : Hub
    {
        private static ConcurrentDictionary<string, string> _connections = new();

        public Task RegisterUser(string userId)
        {
            _connections[userId] = Context.ConnectionId;
            return Task.CompletedTask;
        }

        public static string? GetConnectionId(string userId)
            => _connections.TryGetValue(userId, out var cid) ? cid : null;

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(user.Key))
                _connections.TryRemove(user.Key, out _);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
