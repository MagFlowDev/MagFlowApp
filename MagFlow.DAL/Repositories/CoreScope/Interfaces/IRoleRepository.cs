using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface IRoleRepository : IRepository<ApplicationRole, CoreDbContext>
    {
        Task<List<Claim>> GetAllClaims();

        Task<Dictionary<Guid, List<Claim>>> GetRolesClaims(List<Guid> rolesIds);
        Task<Dictionary<Guid, List<Claim>>> GetRolesClaims(List<string> rolesNames);

        Task<Enums.Result> AddRolesClaims(List<RoleClaim> claims);
        Task<Enums.Result> UpdateRolesClaims(List<RoleClaim> claims);
        Task<Enums.Result> DeleteRolesClaims(List<RoleClaim> claims);
    }
}
