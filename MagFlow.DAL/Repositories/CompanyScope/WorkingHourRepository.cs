using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class WorkingHourRepository : BaseCompanyRepository<DefaultWorkingHour, WorkingHourRepository>, IWorkingHourRepository
    {
        public WorkingHourRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<WorkingHourRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
