using MagFlow.Shared.Models.Enumerators;
using MagFlow.Shared.Models.Interfaces;
using MagFlow.Web.Pages.Modules.Contractors;
using MagFlow.Web.Pages.Modules.Documents;
using MagFlow.Web.Pages.Modules.Logistics;
using MagFlow.Web.Pages.Modules.Machines;
using MagFlow.Web.Pages.Modules.Orders;
using MagFlow.Web.Pages.Modules.Processes;
using MagFlow.Web.Pages.Modules.ProductionPlan;
using MagFlow.Web.Pages.Modules.Reports;
using MagFlow.Web.Pages.Modules.Users;
using MagFlow.Web.Pages.Modules.Warehouses;
using MagFlow.Web.Pages.Modules.Wares;
using MagFlow.Web.Pages.Modules.Worktime;

namespace MagFlow.Web.Helpers;

public static class IModuleMapper
{
    public static IModule? GetIModule(ModuleType? moduleType)
    {
        if (moduleType == null)
            return null;
        var id = moduleType.Id;
        if (id == ModuleType.UsersModule.Id)
            return new Users();
        else if (id == ModuleType.ContractorsModule.Id)
            return new Contractors();
        else if (id == ModuleType.WaresModule.Id)
            return new Wares();
        else if (id == ModuleType.OrdersModule.Id)
            return new Orders();
        else if (id == ModuleType.DocumentsModule.Id)
            return new Documents();
        else if (id == ModuleType.WarehousesModule.Id)
            return new Warehouses();
        else if (id == ModuleType.MachinesModule.Id)
            return new Machines();
        else if (id == ModuleType.ProcessesModule.Id)
            return new Processes();
        else if (id == ModuleType.ReportsModule.Id)
            return new Reports();
        else if (id == ModuleType.WorkTimeModule.Id)
            return new Worktime();
        else if (id == ModuleType.ProductionPlanModule.Id)
            return new ProductionPlan();
        else if (id == ModuleType.LogisticsModule.Id)
            return new Logistics();
        else
            return null;
    }
}