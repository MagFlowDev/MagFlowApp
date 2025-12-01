using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MagFlow.Shared.Models;

namespace MagFlow.BLL.Services.Notifications
{
    public record NotificationMessage(string Message, DateTime Timestamp, Enums.NotificationType Type);

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
                    if (payload?.GetType().GetProperty("Message")?.GetValue(payload) is not null)
                    {
                        var msg = payload?.GetType().GetProperty("Message")?.GetValue(payload)?.ToString() ?? payload?.ToString() ?? string.Empty;
                        var tsObj = payload?.GetType().GetProperty("Timestamp")?.GetValue(payload);
                        var typeObj = payload?.GetType().GetProperty("Type")?.GetValue(payload);
                        var ts = tsObj is DateTime dt ? dt : DateTime.UtcNow;
                        Enums.NotificationType type = Enums.NotificationType.Unknown;
                        if(typeObj is Enums.NotificationType t)
                            Enum.TryParse(typeObj.ToString(), out type);    
                        OnNotificationReceived?.Invoke(new NotificationMessage(msg, ts, type));
                    }
                    else
                    {
                        var msgObj = payload?.ToString() ?? string.Empty;
                        if (!string.IsNullOrEmpty(msgObj))
                        {
                            var notificationMessage = JsonSerializer.Deserialize<NotificationMessage>(msgObj);
                            if(notificationMessage != null)
                                OnNotificationReceived?.Invoke(notificationMessage);
                        }
                    }
                }
                catch
                {
                    OnNotificationReceived?.Invoke(new NotificationMessage(payload?.ToString() ?? string.Empty, DateTime.UtcNow, Enums.NotificationType.Unknown));
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
