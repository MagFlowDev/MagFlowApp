using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.FormModels
{
    public class RegionalSettingsModel
    {
        public Enums.DecimalSeparator DecimalSeparator { get; set; }
        public Enums.DateFormat DateFormat { get; set; }
        public Enums.TimeFormat TimeFormat { get; set; }
        public Enums.TimeZone TimeZone { get; set; }
    }
}
