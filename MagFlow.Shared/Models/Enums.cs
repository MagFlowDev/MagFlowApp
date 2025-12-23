using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models
{
    public static class Enums
    {
        public enum Language
        {
            Polish,
            English
        }

        public enum Result
        {
            Unknown,
            Error,
            Success
        }

        public enum RemovalReason
        {
            Unknown
        }
        
        public enum ValueType
        {
            Unknown
        }
        
        public enum ImpactType
        {
            Unknown
        }
        
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

        public enum ItemStatus
        {
            Unknown
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
            Unknown,
            Logging
        }

        public enum EventLogLevel
        {
            TRACE = 0,
            INFO = 1,
            WARN = 2,
            ERROR = 3,
            FATAL = 4
        }

        public enum AuditLogAction
        {
            Unknown
        }

        public enum NotificationType
        {
            Unknown,
            System,
            User,
            Company
        }

        public enum NotificationEntityType
        {
            Unknown
        }
    }
}
