using MagFlow.DAL.Helpers;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Constants.Identificators;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope
{
    public class UserRepository : BaseCoreRepository<MagFlow.Domain.CoreScope.ApplicationUser, UserRepository>, IUserRepository
    {
        public UserRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<UserRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {

        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    return await context.ApplicationUsers
                        .Include(u => u.UserSettings)
                        .Include(u => u.Roles).ThenInclude(r => r.Role)
                        .FirstOrDefaultAsync(u => u.Email == email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public override async Task<ApplicationUser?> GetByIdAsync(object id, Func<IQueryable<ApplicationUser>, IQueryable<ApplicationUser>>? include = null)
        {
            try
            {
                if (id is not Guid guid)
                    return null;

                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var query = context.ApplicationUsers.AsQueryable();
                    query = query
                        .Include(u => u.UserSettings)
                        .Include(u => u.Roles).ThenInclude(r => r.Role);
                    if (include != null)
                        query = include(query);
                    return await query.FirstOrDefaultAsync(i => i.Id == guid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<List<Claim>> GetUserClaims(Guid userId)
        {
            List<Claim> claims = new List<Claim>();
            try
            {
                var user = await GetByIdAsync(userId);
                if (user != null && user.DefaultCompanyId.HasValue)
                {
                    foreach (var applicationUserRole in user.Roles)
                    {
                        var roleClaims = await GetRoleClaims(applicationUserRole.RoleId, user.DefaultCompanyId.Value);
                        claims.AddRange(roleClaims);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return claims.DistinctBy(x => x.Id).ToList();
        }

        public async Task<List<Claim>> GetRoleClaims(Guid roleId, Guid? companyId = null)
        {
            List<Claim> claims = new List<Claim>();
            try
            {
                if (companyId.HasValue)
                {
                    using (var coreContext = _coreContextFactory.CreateDbContext())
                    {
                        var company = await coreContext.Companies.FirstOrDefaultAsync(x => x.Id == companyId.Value);
                        if (company == null)
                            return claims;
                        using (var context = _companyContextFactory.CreateDbContext(company.ConnectionString))
                        {
                            var roleClaims = await context.RoleClaims
                                .Where(x => x.RoleId == roleId)
                                .Include(x => x.Claim)
                                .Select(x => x.Claim)
                                .ToListAsync();
                            if (roleClaims != null)
                            {
                                return roleClaims.Where(x => x != null).ToList()!;
                            }
                        }
                    }
                    return claims;
                }
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var roleClaims = await context.RoleClaims
                        .Where(x => x.RoleId == roleId)
                        .Include(x => x.Claim)
                        .Select(x => x.Claim)
                        .ToListAsync();
                    if (roleClaims != null)
                    {
                        return roleClaims.Where(x => x != null).ToList()!;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return claims;
        }

        public async Task<List<Claim>> GetRoleClaims(string roleName, Guid? companyId = null)
        {
            List<Claim> claims = new List<Claim>();
            try
            {
                if (companyId.HasValue)
                {
                    using (var coreContext = _coreContextFactory.CreateDbContext())
                    {
                        var company = await coreContext.Companies.FirstOrDefaultAsync(x => x.Id == companyId.Value);
                        if (company == null)
                            return claims;
                        using (var context = _companyContextFactory.CreateDbContext(company.ConnectionString))
                        {
                            var roleClaims = await context.RoleClaims
                                .Where(x => x.RoleName == roleName)
                                .Include(x => x.Claim)
                                .Select(x => x.Claim)
                                .ToListAsync();
                            if (roleClaims != null)
                            {
                                return roleClaims.Where(x => x != null).ToList()!;
                            }
                        }
                    }
                    return claims;
                }
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var roleClaims = await context.RoleClaims
                        .Where(x => x.RoleName == roleName)
                        .Include(x => x.Claim)
                        .Select(x => x.Claim)
                        .ToListAsync();
                    if (roleClaims != null)
                    {
                        return roleClaims.Where(x => x != null).ToList()!;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return claims;
        }

        public async Task<Enums.Result> UpdateSettingsAsync(ApplicationUserSettings settings)
        {
            try
            {
                using(var context = _coreContextFactory.CreateDbContext())
                {
                    context.ApplicationUserSettings.Update(settings);
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

        public async Task<List<UserSession>?> GetLastSessionsAsync(Guid userId, Guid companyId, int historyLength = 1)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {

                    return await context.UserSessions
                        .Where(x => x.UserId == userId && x.CompanyId == companyId && !x.RevokedAt.HasValue && x.ExpiresAt > DateTime.UtcNow)
                        .Include(x => x.SessionModules).ThenInclude(y => y.Module)
                        .OrderByDescending(x => x.LastTimeRecord)
                        .Take(historyLength)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<QueryResponse<ApplicationUser>?> GetCompanyUsersAsync(QueryOptions<User> queryOptions)
        {
            try
            {
                using (var coreContext = _coreContextFactory.CreateDbContext())
                {
                    using (var companyContext = _companyContextFactory.CreateDbContext())
                    {
                        var companyUsersQuery = companyContext.Users
                            .Where(x => x.RemovedAt == null)
                            .ApplyMultiColumnSearch(queryOptions.Search, queryOptions.SearchColumns)
                            .SortBy(queryOptions.SortBy, queryOptions.Descending);
                        var companyUsers = await companyUsersQuery.Paginate(queryOptions.PageNumber, queryOptions.PageSize).ToListAsync();
                        var companyUsersCount = await companyUsersQuery.CountAsync();
                        var companyUsersIds = companyUsers.Select(u => u.Id).ToList();
                        var users = await coreContext.Users
                            .Include(x => x.Companies).ThenInclude(y => y.Company)
                            .Include(x => x.Roles).ThenInclude(y => y.Role)
                            .Where(x => companyUsersIds.Contains(x.Id)).ToListAsync();
                        return new QueryResponse<ApplicationUser>()
                        {
                            Elements = users,
                            TotalCount = companyUsersCount
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }



        public async Task<Enums.Result> DeleteCompanyUserAsync(Guid userId, Guid companyId)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var connectionString = await context.Companies
                        .Where(x => x.Id == companyId)
                        .Select(x => x.ConnectionString)
                        .FirstOrDefaultAsync();
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        using (var companyContext = _companyContextFactory.CreateDbContext())
                        {
                            var companyUser = await companyContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                            if (companyUser == null)
                                return Enums.Result.Error;

                            companyUser.RemovedAt = DateTime.UtcNow;
                            companyContext.Users.Update(companyUser);
                            await companyContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        return Enums.Result.Error;
                    }
                    return Enums.Result.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public async Task<Enums.Result> DeleteCompanyUserAsync(Guid userId)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var userCompanies = await context.CompanyUsers
                        .Where(x => x.UserId == userId)
                        .Select(x => x.CompanyId)
                        .ToListAsync();

                    var connectionStrings = (await context.Companies
                        .Where(x => userCompanies.Contains(x.Id))
                        .ToListAsync())
                        .Select(x => x.ConnectionString);

                    var now = DateTime.UtcNow;
                    foreach (var connectionString in connectionStrings)
                    {
                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            using (var companyContext = _companyContextFactory.CreateDbContext())
                            {
                                var companyUser = await companyContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                                if (companyUser == null)
                                    return Enums.Result.Error;

                                companyUser.RemovedAt = now;
                                companyContext.Users.Update(companyUser);
                                await companyContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            return Enums.Result.Error;
                        }
                    }
                    
                    return Enums.Result.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }
        public async Task<Enums.Result> DeleteCompanyUsersAsync(List<Guid> usersIds)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var userCompanies = await context.CompanyUsers
                        .Where(x => usersIds.Contains(x.UserId))
                        .Select(x => x.CompanyId)
                        .ToListAsync();

                    var connectionStrings = (await context.Companies
                            .Where(x => userCompanies.Contains(x.Id))
                            .ToListAsync())
                        .Select(x => x.ConnectionString);

                    var now = DateTime.UtcNow;
                    foreach (var connectionString in connectionStrings)
                    {
                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            using (var companyContext = _companyContextFactory.CreateDbContext())
                            {
                                var companyUsers = await companyContext.Users
                                    .Where(x => usersIds.Contains(x.Id))
                                    .ToListAsync();
                                if (companyUsers == null)
                                    return Enums.Result.Error;

                                foreach (var companyUser in companyUsers)
                                {
                                    companyUser.RemovedAt = now;
                                }

                                companyContext.Users.UpdateRange(companyUsers);
                                await companyContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            return Enums.Result.Error;
                        }
                    }

                    return Enums.Result.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }
    }
}
