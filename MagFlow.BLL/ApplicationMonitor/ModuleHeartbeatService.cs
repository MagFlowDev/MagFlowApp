using MagFlow.BLL.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.ApplicationMonitor
{
    public class ModuleHeartbeatService : IModuleHeartbeatService
    {
        private readonly Timer _heartbeatTimer;
        private readonly int _heartbeatInterval;
        private readonly ApplicationStateInfo _applicationStateInfo;
        private readonly ILogger<ModuleHeartbeatService> _logger;

        private readonly Subject<string> _connectedServiceHeartbeat = new Subject<string>();
        public IObservable<string> ConnectedServiceHeartbeat => _connectedServiceHeartbeat;

        public ModuleHeartbeatService(
            ILogger<ModuleHeartbeatService> logger,
            ApplicationStateInfo applicationStateInfo,
            int heartbeatInterval)
        {
            _logger = logger;
            _heartbeatInterval = heartbeatInterval;
            _heartbeatTimer = new Timer(_ => SendHeartbeat(), null, Timeout.Infinite, Timeout.Infinite);
            _applicationStateInfo = applicationStateInfo;
        }


        public void StartHeartbeat()
        {
            _heartbeatTimer.Change(0, _heartbeatInterval * 1000);
            SendHeartbeat();
        }
        public void StopHeartbeat()
        {
            _heartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        public void SendHeartbeat(string? serviceId = null)
        {
            _logger.LogInformation($"Heartbeat at {DateTime.UtcNow} UTC");
            if(!string.IsNullOrWhiteSpace(serviceId))
                _connectedServiceHeartbeat.OnNext(serviceId);
            else if(serviceId == null)
            {
                foreach (var service in _applicationStateInfo.ConnectedServices)
                    _connectedServiceHeartbeat.OnNext(service.ServiceId);
            }
        }


        public void AddConnectedService(string serviceId)
        {
            var service = _applicationStateInfo.ConnectedServices.FirstOrDefault(x => x.ServiceId == serviceId);
            if(service != null)
            {
                _logger.LogDebug($"Found existing connected service '{serviceId}' while adding new connected service. The service will be removed.");
                _applicationStateInfo.ConnectedServices.Remove(service);
            }

            service = new ConnectedService { ServiceId = serviceId };
            _applicationStateInfo.ConnectedServices.Add(service);
            _logger.LogInformation($"Added new connected service, serviceId: '{serviceId}'");

            SendHeartbeat(serviceId);
        }

        public void RemoveConnectedService(string serviceId)
        {
            var service = _applicationStateInfo.ConnectedServices.FirstOrDefault(s => s.ServiceId == serviceId);

            if (service == null)
                return;

            _applicationStateInfo.ConnectedServices.Remove(service);
            _logger.LogInformation($"Removed connected service '{serviceId}'");

            SendHeartbeat();
        }
    }
}
