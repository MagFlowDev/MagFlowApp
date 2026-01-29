using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.Core
{
    public class UserRepository : BaseCoreRepository<MagFlow.Domain.Core.ApplicationUser, UserRepository>, IUserRepository
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

        public override async Task<ApplicationUser?> GetByIdAsync(object id)
        {
            try
            {
                if (id is not Guid guid)
                    return null;

                using (var context = _coreContextFactory.CreateDbContext())
                {
                    return await context.ApplicationUsers
                        .Include(u => u.UserSettings)
                        .Include(u => u.Roles).ThenInclude(r => r.Role)
                        .FirstOrDefaultAsync(i => i.Id == guid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<UserSession?> GetLastSessionAsync(Guid userId)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {

                    return await context.UserSessions
                        .Where(x => x.UserId == userId && !x.RevokedAt.HasValue && x.ExpiresAt > DateTime.UtcNow)
                        .Include(x => x.SessionModules).ThenInclude(y => y.Module)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefaultAsync();
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
