using MagFlow.Shared.Constants;
using MagFlow.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Notifications
{
    public record NotificationMessage(string Title, string Message, DateTime Timestamp, Enums.NotificationType Type, DateTime? ExpireAt);

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
                .WithUrl(_navigationManager.ToAbsoluteUri(APP_URL.NOTIFICATION_HUB))
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<object>("ReceiveNotification", payload =>
            {
                try
                {
                    if (payload?.GetType().GetProperty("Message")?.GetValue(payload) is not null)
                    {
                        var msg = payload?.GetType().GetProperty("Message")?.GetValue(payload)?.ToString() ?? payload?.ToString() ?? string.Empty;
                        var title = payload?.GetType().GetProperty("Title")?.GetValue(payload)?.ToString() ?? string.Empty;
                        var tsObj = payload?.GetType().GetProperty("Timestamp")?.GetValue(payload);
                        var typeObj = payload?.GetType().GetProperty("Type")?.GetValue(payload);
                        var expireObj = payload?.GetType().GetProperty("ExpireAt")?.GetValue(payload);
                        var ts = tsObj is DateTime dt ? dt : DateTime.UtcNow;
                        DateTime? expire = expireObj is DateTime edt ? edt : null;
                        Enums.NotificationType type = Enums.NotificationType.Unknown;
                        if(typeObj is Enums.NotificationType t)
                            Enum.TryParse(typeObj.ToString(), out type);    
                        OnNotificationReceived?.Invoke(new NotificationMessage(title, msg, ts, type, expire));
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
                    OnNotificationReceived?.Invoke(new NotificationMessage(string.Empty, payload?.ToString() ?? string.Empty, DateTime.UtcNow, Enums.NotificationType.Unknown, null));
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
