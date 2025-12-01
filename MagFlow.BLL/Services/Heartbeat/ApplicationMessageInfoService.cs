using MagFlow.BLL.ApplicationMonitor;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Heartbeat
{
    public class ApplicationMessageInfoService : IHeartbeatService
    {
        private readonly IServerNotificationService _appNotificationService;
        private readonly INotificationService _notificationService;
        private IApplicationMonitorService _appMonitorService;
        private IDisposable _subscription;

        public ApplicationMessageInfoService(IApplicationMonitorService appMonitorService,
            IServerNotificationService appNotificationService,
            INotificationService notificationService)
        {
            _appMonitorService = appMonitorService;
            _appNotificationService = appNotificationService;
            _notificationService = notificationService;
            _subscription = new CompositeDisposable(
                _appMonitorService.ConnectedServiceHeartbeat.Subscribe(Heartbeat));

            InitializeService();
        }

        public void InitializeService()
        {
            _appMonitorService.AddConnectedService(ServiceId.ApplicationMessageInfoServiceId);
        }

        public void Heartbeat(string serviceId)
        {
            if (serviceId != ServiceId.ApplicationMessageInfoServiceId)
                return;

            Task.Run(async () => await DisplaySystemNotifications());
        }

        private async Task DisplaySystemNotifications()
        {
            var notifications = await _notificationService.GetCurrentSystemNotificationsAsync();
            // await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new { Message = "tekst", Timestamp = DateTime.UtcNow });
            await _appNotificationService.NotifyAllAsync("test message");
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _appMonitorService.RemoveConnectedService(ServiceId.ApplicationMessageInfoServiceId);
        }
    }
}
