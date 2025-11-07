using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.Settings
{
    /// <summary>
    /// Open Telemetry settings
    /// </summary>
    public class OtelSettings
    {
        public bool Enabled { get; set; }
        public string Address { get; set; }
    }
}
