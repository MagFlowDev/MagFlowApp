using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models
{
    public static class Enums
    {
        public enum Condition
        {
            Unknown
        }
        
        public enum ProcessOriginType
        {
            Unknown
        }
        
        public enum IODirection
        {
            In,
            Out
        }

        public enum DocDirection
        {
            In,
            Out
        }
        
        public enum OverallState
        {
            Unknown = 0,
            Operative = 1,
            Inoperative = 2,
        }

        public enum DocumentStatus
        {
            Unknown
        }
        
        public enum ProcessStatus
        {
            Unknown
        }
        
        public enum OrderStatus
        {
            Unknown
        }

        public enum ContractorType
        {
            Unknown
        }

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
