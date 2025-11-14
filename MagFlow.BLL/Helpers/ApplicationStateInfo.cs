using MagFlow.BLL.ApplicationMonitor;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Helpers
{
    public class ApplicationStateInfo
    {
        public Enums.OverallState OverallState { get; set; }
        public List<ConnectedService> ConnectedServices { get; set; }
        public int ProcessId { get; set; }

        public ApplicationStateInfo()
        {
            ConnectedServices = new List<ConnectedService>();
            var process = Process.GetCurrentProcess();
            ProcessId = process.Id;
        }
    }
}
