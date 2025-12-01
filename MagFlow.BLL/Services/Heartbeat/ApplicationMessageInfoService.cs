using MagFlow.BLL.ApplicationMonitor;
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
        private IApplicationMonitorService _appMonitorService;

        private IDisposable _subscription;

        public ApplicationMessageInfoService(IApplicationMonitorService appMonitorService)
        {
            _appMonitorService = appMonitorService;

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
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _appMonitorService.RemoveConnectedService(ServiceId.ApplicationMessageInfoServiceId);
        }
    }
}
