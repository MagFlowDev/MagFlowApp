using MagFlow.DAL.Repositories.Company.Interfaces;
using MagFlow.Domain.Company;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.Company
{
    public class UnitRepository : BaseCompanyRepository<Unit, UnitRepository>, IUnitRepository
    {
        public UnitRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<UnitRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
