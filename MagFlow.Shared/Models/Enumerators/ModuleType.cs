using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.Enumerators
{
    public class ModuleType : Enumeration
    {
        public static ModuleType UsersModule = new(1, nameof(UsersModule));
        public static ModuleType ContractorsModule = new(2, nameof(ContractorsModule));
        public static ModuleType WaresModule = new(3, nameof(WaresModule));
        public static ModuleType OrdersModule = new(4, nameof(OrdersModule));
        public static ModuleType DocumentsModule = new(5, nameof(DocumentsModule));
        public static ModuleType WarehousesModule = new(6, nameof(WarehousesModule));
        public static ModuleType MachinesModule = new(7, nameof(MachinesModule));
        public static ModuleType ProcessesModule = new(8, nameof(ProcessesModule));
        public static ModuleType RaportsModule = new(9, nameof(RaportsModule));
        public static ModuleType WorkTimeModule = new(10, nameof(WorkTimeModule));
        public static ModuleType ProductionPlanModule = new(11, nameof(ProductionPlanModule));
        public static ModuleType LogisticsModule = new(12, nameof(LogisticsModule));

        public ModuleType(int id, string name) : base(id, name)
        {
        }
    }
}
