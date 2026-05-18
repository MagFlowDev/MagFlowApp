using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class ProductParameterRepository : BaseCompanyRepository<ProductParameter, ProductParameterRepository>, IProductParameterRepository
    {
        public ProductParameterRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<ProductParameterRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
