using MagFlow.BLL.ApplicationMonitor;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.Constants;
using MagFlow.Shared.Models;
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
        private readonly IServerNotificationService _serverNotificationService;
        private readonly INotificationService _notificationService;
        private IApplicationMonitorService _appMonitorService;
        private IDisposable _subscription;

        public ApplicationMessageInfoService(IApplicationMonitorService appMonitorService,
            IServerNotificationService serverNotificationService,
            INotificationService notificationService)
        {
            _appMonitorService = appMonitorService;
            _serverNotificationService = serverNotificationService;
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
            Task.Run(async () => await DisplayCompanyNotifications());
        }

        private async Task DisplaySystemNotifications()
        {
            var notifications = await _notificationService.GetCurrentSystemNotificationsAsync();
            var notification = notifications.OrderByDescending(exp => exp.ExpireAt).FirstOrDefault();
            if (notification == null)
                return;
            await _serverNotificationService.NotifyAllAsync(notification.Title, notification.Message, Enums.NotificationType.System, notification.ExpireAt);
        }

        private async Task DisplayCompanyNotifications()
        {
            
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _appMonitorService.RemoveConnectedService(ServiceId.ApplicationMessageInfoServiceId);
        }
    }
}
