using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CoreScope
{
    public class CompanyRepository : BaseCoreRepository<MagFlow.Domain.CoreScope.Company, CompanyRepository>, ICompanyRepository
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

        public async Task<Enums.Result> UpdateLogoAsync(Guid companyId, byte[] data, string contentType)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var existingLogo = await context.CompanyLogo.FirstOrDefaultAsync(x => x.CompanyId == companyId);
                    if (existingLogo == null)
                    {
                        var newLogo = new CompanyLogo()
                        {
                            CompanyId = companyId,
                            ImageData = data,
                            ContentType = contentType
                        };
                        await context.CompanyLogo.AddAsync(newLogo);
                    }
                    else
                    {
                        existingLogo.ImageData = data;
                        existingLogo.ContentType = contentType;
                        context.CompanyLogo.Update(existingLogo);
                    }
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public async Task<Enums.Result> RemoveLogoAsync(Guid companyId)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var existingLogo = await context.CompanyLogo.FirstOrDefaultAsync(x => x.CompanyId == companyId);
                    if (existingLogo == null)
                        return Enums.Result.Success;
                    else
                    {
                        context.CompanyLogo.Remove(existingLogo);
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

        public async Task<Enums.Result> UpdateSettingsAsync(CompanySettings settings)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    context.CompanySettings.Update(settings);
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

        public override Enums.Result Delete(Company entity)
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

        public override async Task<Enums.Result> DeleteAsync(Company entity)
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

        public override Enums.Result DeleteMany(Expression<Func<Company, bool>> predicate)
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

        public override async Task<Enums.Result> DeleteManyAsync(Expression<Func<Company, bool>> predicate)
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
