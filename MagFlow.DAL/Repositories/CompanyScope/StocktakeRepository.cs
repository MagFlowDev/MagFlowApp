using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class StocktakeRepository : BaseCompanyRepository<Stocktake, StocktakeRepository>, IStocktakeRepository
    {
        public StocktakeRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<StocktakeRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
