using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class MachineRepository : BaseCompanyRepository<Machine, MachineRepository>, IMachineRepository
    {
        public MachineRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<MachineRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
