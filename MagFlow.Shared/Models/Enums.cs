using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models
{
    public static class Enums
    {
        public enum Currency
        {
            PLN = 0,
            EURO = 1,
            USD = 2
        }

        public enum EventLogCategory
        {
            Unknown
        }

        public enum EventLogLevel
        {
            Unknown
        }

        public enum AuditLogAction
        {
            Unknown
        }

        public enum NotificationType
        {
            Unknown
        }

        public enum NotificationEntityType
        {
            Unknown
        }
    }
}
