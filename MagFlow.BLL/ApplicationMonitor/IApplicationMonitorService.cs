using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.ApplicationMonitor
{
    public interface IApplicationMonitorService
    {
        IObservable<string> ConnectedServiceHeartbeat { get; }

        void StartHeartbeat();
        void StopHeartbeat();
        void SendHeartbeat(string? serviceId = null);

        void AddConnectedService(string serviceId);
        void RemoveConnectedService(string serviceId);
    }
}
