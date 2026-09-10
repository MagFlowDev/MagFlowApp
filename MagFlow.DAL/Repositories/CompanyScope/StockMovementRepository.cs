using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class StockMovementRepository : BaseCompanyRepository<StockMovement, StockMovementRepository>, IStockMovementRepository
    {
        public StockMovementRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<StockMovementRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
