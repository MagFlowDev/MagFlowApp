using MagFlow.Shared.Constants.Identificators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.Enumerators
{
    public class ModuleType : Enumeration<Guid>
    {
        public static ModuleType UsersModule = new(ModuleID.USERS_MODULE_ID, nameof(UsersModule));
        public static ModuleType ContractorsModule = new(ModuleID.CONTRACTORS_MODULE_ID, nameof(ContractorsModule));
        public static ModuleType WaresModule = new(ModuleID.WARES_MODULE_ID, nameof(WaresModule));
        public static ModuleType OrdersModule = new(ModuleID.ORDERS_MODULE_ID, nameof(OrdersModule));
        public static ModuleType DocumentsModule = new(ModuleID.DOCUMENTS_MODULE_ID, nameof(DocumentsModule));
        public static ModuleType WarehousesModule = new(ModuleID.WAREHOUSES_MODULE_ID, nameof(WarehousesModule));
        public static ModuleType MachinesModule = new(ModuleID.MACHINES_MODULE_ID, nameof(MachinesModule));
        public static ModuleType ProcessesModule = new(ModuleID.PROCESSES_MODULE_ID, nameof(ProcessesModule));
        public static ModuleType RaportsModule = new(ModuleID.RAPORTS_MODULE_ID, nameof(RaportsModule));
        public static ModuleType WorkTimeModule = new(ModuleID.WORKTIME_MODULE_ID, nameof(WorkTimeModule));
        public static ModuleType ProductionPlanModule = new(ModuleID.PRODUCTION_PLAN_MODULE_ID, nameof(ProductionPlanModule));
        public static ModuleType LogisticsModule = new(ModuleID.LOGISTICS_MODULE_ID, nameof(LogisticsModule));

        public ModuleType(Guid id, string name) : base(id, name)
        {
        }
    }
}
