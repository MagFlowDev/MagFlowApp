using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.EF;
using MagFlow.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagFlow.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.DAL.Repositories.Core
{
    public class CompanyRepository : BaseCoreRepository<MagFlow.Domain.Core.Company, CompanyRepository>, ICompanyRepository
    {
        public CompanyRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<CompanyRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
            
        }

        public async Task<List<CompanyModule>?> GetCompanyModules(Guid companyId)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var company = await context.Companies
                        .Where(x => x.Id == companyId)
                        .Include(x => x.Modules).ThenInclude(y => y.Module)
                        .FirstOrDefaultAsync();
                    return company?.Modules
                        .Where(x => x.IsActive && x.EnabledTo > DateTime.UtcNow)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        
        public override Enums.Result Delete(Domain.Core.Company entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext(entity.ConnectionString))
                {
                    context.Database.EnsureDeleted();
                }
                return base.Delete(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public override async Task<Enums.Result> DeleteAsync(Domain.Core.Company entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext(entity.ConnectionString))
                {
                    await context.Database.EnsureDeletedAsync();
                }
                return await base.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public override Enums.Result DeleteMany(Expression<Func<Domain.Core.Company, bool>> predicate)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var entities = Find(predicate);
                    if (entities == null)
                        return Enums.Result.Error;
                    
                    foreach (var entity in entities)
                    {
                        try
                        {
                            using (var dbContext = _companyContextFactory.CreateDbContext(entity.ConnectionString))
                            {
                                context.Database.EnsureDeleted();
                            }
                            context.Companies.Remove(entity);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }
                    }

                    context.SaveChanges();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public override async Task<Enums.Result> DeleteManyAsync(Expression<Func<Domain.Core.Company, bool>> predicate)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var entities = Find(predicate);
                    if (entities == null)
                        return Enums.Result.Error;

                    foreach (var entity in entities)
                    {
                        try
                        {
                            using (var dbContext = _companyContextFactory.CreateDbContext(entity.ConnectionString))
                            {
                                await context.Database.EnsureDeletedAsync();
                            }
                            context.Companies.Remove(entity);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }
                    }

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
