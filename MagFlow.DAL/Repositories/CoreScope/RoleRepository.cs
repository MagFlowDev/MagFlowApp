using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope
{
    public class RoleRepository : BaseCoreRepository<ApplicationRole, RoleRepository>, IRoleRepository
    {
        public RoleRepository(ICoreDbContextFactory coreContextFactory,
           ICompanyDbContextFactory companyContextFactory,
           ILogger<RoleRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {

        }
    }
}
