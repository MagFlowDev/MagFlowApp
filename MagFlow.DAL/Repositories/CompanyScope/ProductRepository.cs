using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Enums.Result> RemoveProductParameters(List<ProductParameter> parameters)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.ProductParameters.RemoveRange(parameters);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public async Task<Enums.Result> RemoveProductComponents(List<ProductComponent> components)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.ProductComponents.RemoveRange(components);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }
    }
}
