using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class ProductTypeRepository : BaseCompanyRepository<ProductType, ProductTypeRepository>, IProductTypeRepository
    {
        public ProductTypeRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<ProductTypeRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
