using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class ParameterRepository : BaseCompanyRepository<CustomParameter, ParameterRepository>, IParameterRepository
    {
        public ParameterRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<ParameterRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
