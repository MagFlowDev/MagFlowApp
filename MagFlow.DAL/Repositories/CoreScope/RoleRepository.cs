using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope
{
    public class RoleRepository : BaseCoreRepository<ApplicationRole, RoleRepository>, IRoleRepository
    {
        public RoleRepository(ICoreDbContextFactory coreContextFactory,
           ICompanyDbContextFactory companyContextFactory,
           ILogger<RoleRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {

        }

        public async Task<List<Claim>> GetAllClaims()
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return await context.Claims.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new List<Claim>();
        }

        public async Task<Dictionary<Guid, List<Claim>>> GetRolesClaims(List<Guid> rolesIds)
        {
            var rolesClaims = new Dictionary<Guid, List<Claim>>();
            try
            {
                if (!rolesIds.Any())
                    return rolesClaims;
                using(var context = _companyContextFactory.CreateDbContext())
                {
                    var rolesClaimsEntities = await context.RoleClaims
                        .Include(x => x.Claim)
                        .Where(x => rolesIds.Contains(x.RoleId))
                        .GroupBy(x => x.RoleId)
                        .ToListAsync();
                    rolesClaims = rolesClaimsEntities.ToDictionary(x => x.Key, x => x.Where(y => y.Claim != null).Select(y => y.Claim!).ToList());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return rolesClaims;
        }

        public async Task<Dictionary<Guid, List<Claim>>> GetRolesClaims(List<string> rolesNames)
        {
            var rolesClaims = new Dictionary<Guid, List<Claim>>();
            try
            {
                List<Guid> rolesIds = new List<Guid>();
                using(var context = _coreContextFactory.CreateDbContext())
                {
                    var roles = context.ApplicationRoles.Where(x => !string.IsNullOrEmpty(x.Name) && rolesNames.Contains(x.Name));
                    rolesIds = roles.Select(x => x.Id).ToList();
                }
                return await GetRolesClaims(rolesIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return rolesClaims;
        }

        public async Task<Enums.Result> AddRolesClaims(List<RoleClaim> claims)
        {
            try
            {
                if (!claims.Any())
                    return Enums.Result.Success;

                using(var context = _companyContextFactory.CreateDbContext())
                {
                    await context.RoleClaims.AddRangeAsync(claims);
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

        public async Task<Enums.Result> UpdateRolesClaims(List<RoleClaim> claims)
        {
            try
            {
                if (!claims.Any())
                    return Enums.Result.Success;

                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.RoleClaims.UpdateRange(claims);
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

        public async Task<Enums.Result> DeleteRolesClaims(List<RoleClaim> claims)
        {
            try
            {
                if (!claims.Any())
                    return Enums.Result.Success;

                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.RoleClaims.RemoveRange(claims);
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
