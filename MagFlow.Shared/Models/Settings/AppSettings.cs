using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.Settings
{
    public static class AppSettings
    {
        public static ConnectionStrings ConnectionStrings;
        public static OtelSettings OtelSettings;
        public static SmtpSettings SmtpSettings;
        public static Uri AppUri;
    }
}
