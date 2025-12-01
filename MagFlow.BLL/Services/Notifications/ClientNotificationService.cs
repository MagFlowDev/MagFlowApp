using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Notifications
{
    public record NotificationMessage(string Message, DateTime Timestamp);

    public class ClientNotificationService : IAsyncDisposable
    {
        private readonly NavigationManager _navigationManager;
        private HubConnection? _hubConnection;

        public event Action<NotificationMessage>? OnNotificationReceived;
        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public ClientNotificationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public Task StartAsync()
        {
            if (_hubConnection is not null)
                return Task.CompletedTask;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("/hubs/notifications"))
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<object>("ReceiveNotification", payload =>
            {
                try
                {
                    var msg = payload?.GetType().GetProperty("Message")?.GetValue(payload)?.ToString() ?? payload?.ToString() ?? string.Empty;
                    var tsObj = payload?.GetType().GetProperty("Timestamp")?.GetValue(payload);
                    var ts = tsObj is DateTime dt ? dt : DateTime.UtcNow;
                    OnNotificationReceived?.Invoke(new NotificationMessage(msg, ts));
                }
                catch
                {
                    OnNotificationReceived?.Invoke(new NotificationMessage(payload?.ToString() ?? string.Empty, DateTime.UtcNow));
                }
            });

            return _hubConnection.StartAsync();
        }

        public Task StopAsync()
        {
            if (_hubConnection is null)
                return Task.CompletedTask;

            return _hubConnection.StopAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }
    }
}
