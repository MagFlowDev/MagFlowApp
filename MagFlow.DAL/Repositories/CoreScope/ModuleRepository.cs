using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope
{
    public class ModuleRepository : BaseCoreRepository<Module, ModuleRepository>, IModuleRepository
    {
        public ModuleRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<ModuleRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
