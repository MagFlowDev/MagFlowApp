using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.Core
{
    public class CompanyRepository : BaseCoreRepository<MagFlow.Domain.Core.Company, CompanyRepository>, ICompanyRepository
    {
        public CompanyRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<CompanyRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
            
        }
    }
}
