using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Constants
{
    public static class Policies
    {
        public const string ADMIN_ONLY = "AdminOnly";
        public const string COMPANY_ADMIN = "CompanyAdminOnly";

        public const string CAN_ACCESS_USERS = "CanAccessUsers";
        public const string USER_ADD_OR_ADMIN = "User.Edit.OrAdmin";

        public static class Claims
        {
            public const string WORKTIME_READ = "WorkTime.Read";
            public const string WORKTIME_EDIT = "WorkTime.Edit";
            public const string WORKTIME_ADD = "WorkTime.Add";
            public const string WORKTIME_DELETE = "WorkTime.Delete";
            public const string WORKTIME_ADMIN = "WorkTime.Admin";

            public const string PROCESSES_READ = "Processes.Read";
            public const string PROCESSES_EDIT = "Processes.Edit";
            public const string PROCESSES_ADD = "Processes.Add";
            public const string PROCESSES_DELETE = "Processes.Delete";
            public const string PROCESSES_ADMIN = "Processes.Admin";

            public const string REPORTS_READ = "Reports.Read";
            public const string REPORTS_EDIT = "Reports.Edit";
            public const string REPORTS_ADD = "Reports.Add";
            public const string REPORTS_DELETE = "Reports.Delete";
            public const string REPORTS_ADMIN = "Reports.Admin";

            public const string LOGISTICS_READ = "Logistics.Read";
            public const string LOGISTICS_EDIT = "Logistics.Edit";
            public const string LOGISTICS_ADD = "Logistics.Add";
            public const string LOGISTICS_DELETE = "Logistics.Delete";
            public const string LOGISTICS_ADMIN = "Logistics.Admin";

            public const string USERS_READ = "Users.Read";
            public const string USERS_EDIT = "Users.Edit";
            public const string USERS_ADD = "Users.Add";
            public const string USERS_DELETE = "Users.Delete";
            public const string USERS_ADMIN = "Users.Admin";

            public const string WAREHOUSES_READ = "Warehouses.Read";
            public const string WAREHOUSES_EDIT = "Warehouses.Edit";
            public const string WAREHOUSES_ADD = "Warehouses.Add";
            public const string WAREHOUSES_DELETE = "Warehouses.Delete";
            public const string WAREHOUSES_ADMIN = "Warehouses.Admin";

            public const string CONTRACTORS_READ = "Contractors.Read";
            public const string CONTRACTORS_EDIT = "Contractors.Edit";
            public const string CONTRACTORS_ADD = "Contractors.Add";
            public const string CONTRACTORS_DELETE = "Contractors.Delete";
            public const string CONTRACTORS_ADMIN = "Contractors.Admin";

            public const string ORDERS_READ = "Orders.Read";
            public const string ORDERS_EDIT = "Orders.Edit";
            public const string ORDERS_ADD = "Orders.Add";
            public const string ORDERS_DELETE = "Orders.Delete";
            public const string ORDERS_ADMIN = "Orders.Admin";

            public const string WARES_READ = "Wares.Read";
            public const string WARES_EDIT = "Wares.Edit";
            public const string WARES_ADD = "Wares.Add";
            public const string WARES_DELETE = "Wares.Delete";
            public const string WARES_ADMIN = "Wares.Admin";

            public const string DOCUMENTS_READ = "Documents.Read";
            public const string DOCUMENTS_EDIT = "Documents.Edit";
            public const string DOCUMENTS_ADD = "Documents.Add";
            public const string DOCUMENTS_DELETE = "Documents.Delete";
            public const string DOCUMENTS_ADMIN = "Documents.Admin";

            public const string PRODUCTIONPLAN_READ = "ProductionPlan.Read";
            public const string PRODUCTIONPLAN_EDIT = "ProductionPlan.Edit";
            public const string PRODUCTIONPLAN_ADD = "ProductionPlan.Add";
            public const string PRODUCTIONPLAN_DELETE = "ProductionPlan.Delete";
            public const string PRODUCTIONPLAN_ADMIN = "ProductionPlan.Admin";

            public const string MACHINES_READ = "Machines.Read";
            public const string MACHINES_EDIT = "Machines.Edit";
            public const string MACHINES_ADD = "Machines.Add";
            public const string MACHINES_DELETE = "Machines.Delete";
            public const string MACHINES_ADMIN = "Machines.Admin";
        }
    }
}
