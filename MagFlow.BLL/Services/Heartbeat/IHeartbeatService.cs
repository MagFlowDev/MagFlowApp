using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Heartbeat
{
    public interface IHeartbeatService : IDisposable
    {
        void InitializeService();
        void Heartbeat(string serviceId);
    }
}
