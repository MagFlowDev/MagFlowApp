using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.DAL.Helpers;
using MagFlow.Domain.CompanyScope;

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

        public async Task<List<UserSession>?> GetLastSessionsAsync(Guid userId, int historyLength = 1)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {

                    return await context.UserSessions
                        .Where(x => x.UserId == userId && !x.RevokedAt.HasValue && x.ExpiresAt > DateTime.UtcNow)
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
    }
}
