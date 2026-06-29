using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagFlow.Shared.Models
{
    public static class Enums
    {
        public enum TaxRate
        {
            _0 = 0,
            _5 = 5,
            _8 = 8,
            _23 = 23
        }

        public enum DayOfWeek
        {
            [Display(Name = "Monday")]
            Monday = 0,

            [Display(Name = "Tuesday")]
            Tuesday = 1,

            [Display(Name = "Wednesday")]
            Wednesday = 2,

            [Display(Name = "Thursday")]
            Thursday = 3,

            [Display(Name = "Friday")]
            Friday = 4,

            [Display(Name = "Saturday")]
            Saturday = 5,

            [Display(Name = "Sunday")]
            Sunday = 6,
        }

        public enum ThemeMode
        {
            LightMode,
            DarkMode
        }

        public enum Language
        {
            [Display(Name = "Polish")]
            Polish,

            [Display(Name = "English")]
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
            [Display(Name = "Integer")]
            Integer,

            [Display(Name = "Decimal")]
            Decimal,

            [Display(Name = "Text")]
            Text
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
            [Display(Name = "UnknownStatus")]
            Unknown,

            [Display(Name = "Available")]
            Available,

            [Display(Name = "Blocked")]
            Blocked
        }

        public enum ProductStatus
        {
            Active,
            Inactive,
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
            [Display(Name = "CurrencyPLN")]
            PLN = 0,

            [Display(Name = "CurrencyEURO")]
            EURO = 1,

            [Display(Name = "CurrencyUSD")]
            USD = 2
        }

        public enum DecimalSeparator
        {
            [Display(Name = "DigitSeparatorCommaExample")]
            Comma,

            [Display(Name = "DigitSeparatorDotExample")]
            Dot
        }

        public enum LongTimePeriod
        {
            [Display(Name = "Month")]
            Month = 0,

            [Display(Name = "Quarter")]
            Quarter = 1,

            [Display(Name = "Year")]
            Year = 2
        }

        public enum DateFormat
        {
            [Display(Name = "DateFormatExample1")]
            DD_MM_RRRR_DOTS,

            [Display(Name = "DateFormatExample2")]
            MM_DD_RRRR_SLASHES,

            [Display(Name = "DateFormatExample3")]
            RRRR_MM_DD_DASHES,

            [Display(Name = "DateFormatExample4")]
            DD_MM_RRRR_DASHES,
        }

        public enum TimeFormat
        {
            [Display(Name = "TimeFormatExample1")]
            HH_MM_24H,

            [Display(Name = "TimeFormatExample2")]
            HH_MM_12H,
        }

        public enum TimeZone
        {
            [Display(Name = "TimeZoneUTC")]
            UTC,

            [Display(Name = "TimeZoneEuropeWarsaw")]
            Europe_Warsaw
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

    public static class SectionsEnums
    {
        public enum WaresDefinitionSection
        {
            Categories,
            Types,
            Parameters,
            Units
        }

        public enum WaresModuleSection
        {
            WaresList,
            ProductsList,
            Definitions,
            Archive
        }

        public enum UserDetailsSection
        {
            Profile,
            Journal
        }

        public enum UsersModuleSection
        {
            UsersList,
            RolesList
        }

        public enum CompanySettingsSection
        {
            Information,
            Logo,
            Contact,
            Modules,
            WorkTime
        }

        public enum UserSettingsSection
        {
            Account,
            Password,
            Regional,
            Language,
            Theme,
            Notifications
        }
    }
}
