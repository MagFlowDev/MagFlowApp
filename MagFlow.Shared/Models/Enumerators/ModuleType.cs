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
        public static ModuleType ReportsModule = new(ModuleID.REPORTS_MODULE_ID, nameof(ReportsModule));
        public static ModuleType WorkTimeModule = new(ModuleID.WORKTIME_MODULE_ID, nameof(WorkTimeModule));
        public static ModuleType ProductionPlanModule = new(ModuleID.PRODUCTION_PLAN_MODULE_ID, nameof(ProductionPlanModule));
        public static ModuleType LogisticsModule = new(ModuleID.LOGISTICS_MODULE_ID, nameof(LogisticsModule));

        public ModuleType(Guid id, string name) : base(id, name)
        {
        }

        public static string? GetModuleTypeDescription(ModuleType? moduleType)
        {
            if(moduleType == null)
                return null;
            if (ModuleDescriptionMap.ContainsKey(moduleType))
                return null;
            return ModuleDescriptionMap[moduleType];
        }
        
        public static ModuleType? GetModuleType(string moduleName)
        {
            if(string.IsNullOrEmpty(moduleName))
                return null;
            moduleName = moduleName.ToLower();
            if(ModuleTypeMap.ContainsKey(moduleName))
                return ModuleTypeMap[moduleName];
            return null;
        }
        
        private static Dictionary<ModuleType, string> ModuleDescriptionMap = new Dictionary<ModuleType, string>()
        {
            { ModuleType.ContractorsModule, "ContractorsModuleDescription" },
            { ModuleType.UsersModule, "UsersModuleDescription" },
            { ModuleType.WaresModule, "WaresModuleDescription" },
            { ModuleType.OrdersModule, "OrdersModuleDescription" },
            { ModuleType.DocumentsModule, "DocumentsModuleDescription" },
            { ModuleType.WarehousesModule, "WarehousesModuleDescription" },
            { ModuleType.MachinesModule, "MachinesModuleDescription" },
            { ModuleType.ProcessesModule, "ProcessesModuleDescription" },
            { ModuleType.ReportsModule, "ReportsModuleDescription" },
            { ModuleType.WorkTimeModule, "WorkTimeModuleDescription" },
            { ModuleType.ProductionPlanModule, "ProductionPlanModuleDescription" },
            { ModuleType.LogisticsModule, "LogisticsModuleDescription" },
        };
        
        private static Dictionary<string, ModuleType> ModuleTypeMap = new Dictionary<string, ModuleType>()
        {
            { "contractorsmodule", ModuleType.ContractorsModule },
            { "usersmodule", ModuleType.UsersModule },
            { "waresmodule", ModuleType.WaresModule },
            { "ordersmodule", ModuleType.OrdersModule },
            { "documentsmodule", ModuleType.DocumentsModule },
            { "warehousesmodule", ModuleType.WarehousesModule },
            { "machinesmodule", ModuleType.MachinesModule },
            { "processesmodule", ModuleType.ProcessesModule },
            { "reportsmodule", ModuleType.ReportsModule },
            { "worktimemodule", ModuleType.WorkTimeModule },
            { "productionplanmodule", ModuleType.ProductionPlanModule },
            { "logisticsmodule", ModuleType.LogisticsModule },
        };
    }
}
