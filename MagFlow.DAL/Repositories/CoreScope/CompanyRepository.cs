using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.EF.Seeds.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public async Task<Enums.Result> AddCompanyUser(Company company, User user)
        {
            try
            {
                if (user.Id == Guid.Empty || string.IsNullOrEmpty(user.Email))
                    return Enums.Result.Error;

                using (var context = new CompanyDbContext(company.ConnectionString))
                {
                    if(await context.Users.AnyAsync(x => x.Id == user.Id))
                        return Enums.Result.Error;

                    context.Users.Add(user);
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

        public override async Task<Enums.Result> AddAsync(Company entity, CoreDbContext? context = null)
        {
            try
            {
                var result = await base.AddAsync(entity, context);
                if(result != Enums.Result.Success)
                    return result;

                using (var companyDbContext = new CompanyDbContext(entity.ConnectionString))
                {
                    await companyDbContext.Database.MigrateAsync();
                    await CompanyDbSeeder.SeedAsync(companyDbContext, CancellationToken.None);
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error; ;
            }
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

        public async Task<Enums.Result> UpdateCompanyModules(IEnumerable<CompanyModule> modules)
        {
            try
            {
                if (!modules.Any())
                    return Enums.Result.Success;
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    context.CompanyModules.UpdateRange(modules);
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

        public async Task<Enums.Result> AddCompanyModules(IEnumerable<CompanyModule> modules)
        {
            try
            {
                if (!modules.Any())
                    return Enums.Result.Success;
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    context.CompanyModules.AddRange(modules);
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

        public override Enums.Result Delete(Company entity, CoreDbContext? coreDbContext = null)
        {
            try
            {
                return base.Delete(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public List<Guid>? RemoveAllUsersFromCompany(Company entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.ConnectionString))
                    return null;

                using (var coreContext = _coreContextFactory.CreateDbContext())
                {
                    using (var companyContext = new CompanyDbContext(entity.ConnectionString))
                    {
                        var companyUsersIds = companyContext.Users
                            .Select(u => u.Id)
                            .ToList();

                        coreContext.Users
                            .Where(x => companyUsersIds.Contains(x.Id))
                            .ExecuteUpdate(setters => setters.SetProperty(u => u.DefaultCompanyId, u => null));

                        coreContext.CompanyUsers
                            .Where(x => x.CompanyId == entity.Id)
                            .ExecuteDelete();

                        coreContext.SaveChanges();
                        return companyUsersIds;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public override async Task<Enums.Result> DeleteAsync(Company entity, CoreDbContext? coreDbContext = null)
        {
            try
            {
                return await base.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public async Task<List<Guid>?> RemoveAllUsersFromCompanyAsync(Company entity)
        {
            try
            {
                if(string.IsNullOrEmpty(entity.ConnectionString))
                    return null;

                using (var coreContext = _coreContextFactory.CreateDbContext())
                {
                    using (var companyContext = new CompanyDbContext(entity.ConnectionString))
                    {
                        var companyUsersIds = await companyContext.Users
                            .Select(u => u.Id)
                            .ToListAsync();

                        await coreContext.Users
                            .Where(x => companyUsersIds.Contains(x.Id))
                            .Include(x => x.Companies).ThenInclude(y => y.Company)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.DefaultCompanyId, u => null));

                        await coreContext.CompanyUsers
                            .Where(x => x.CompanyId == entity.Id)
                            .ExecuteDeleteAsync();

                        await coreContext.SaveChangesAsync();
                        return companyUsersIds;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public override Enums.Result DeleteMany(Expression<Func<Company, bool>> predicate, CoreDbContext? coreDbContext = null)
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
                            entity.RemovedAt = DateTime.UtcNow;
                            context.Companies.Update(entity);
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

        public override async Task<Enums.Result> DeleteManyAsync(Expression<Func<Company, bool>> predicate, CoreDbContext? coreDbContext = null)
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
                            entity.RemovedAt = DateTime.UtcNow;
                            context.Companies.Update(entity);
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
