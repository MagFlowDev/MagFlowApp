using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class WorkDayRepository : BaseCompanyRepository<WorkDay, WorkDayRepository>, IWorkDayRepository
    {
        public WorkDayRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<WorkDayRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
