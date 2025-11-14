using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.ApplicationMonitor
{
    public class ApplicationMonitorService : IApplicationMonitorService
    {
        private readonly IModuleHeartbeatService _moduleHeartbeatService;

        public ApplicationMonitorService(IModuleHeartbeatService moduleHeartbeatService, 
            TimeSpan heartbeatTimeout)
        {
            _moduleHeartbeatService = moduleHeartbeatService;
        }

        public IObservable<string> ConnectedServiceHeartbeat => _moduleHeartbeatService.ConnectedServiceHeartbeat;
        public void StartHeartbeat() => _moduleHeartbeatService.StartHeartbeat();
        public void StopHeartbeat() => _moduleHeartbeatService.StopHeartbeat();
        public void SendHeartbeat(string? serviceId = null) => _moduleHeartbeatService.SendHeartbeat(serviceId);
        public void AddConnectedService(string serviceId) => _moduleHeartbeatService.AddConnectedService(serviceId);
        public void RemoveConnectedService(string serviceId) => _moduleHeartbeatService.RemoveConnectedService(serviceId);
    }
}
