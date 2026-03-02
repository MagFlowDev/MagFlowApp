using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class ProductRepository : BaseCompanyRepository<Product, ProductRepository>, IProductRepository
    {
        public ProductRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<ProductRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
