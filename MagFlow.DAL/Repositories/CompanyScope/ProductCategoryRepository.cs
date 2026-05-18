using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class ProductCategoryRepository : BaseCompanyRepository<ProductCategory, ProductCategoryRepository>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<ProductCategoryRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
