using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<ClaimDTO>> GetAllClaims();

        Task<Dictionary<Guid, List<ClaimDTO>>> GetRolesClaims(List<Guid> rolesIds);
        Task<Dictionary<Guid, List<ClaimDTO>>> GetRolesClaims(List<string> rolesNames);

        Task<Enums.Result> UpdateRolesClaims(Dictionary<Guid, List<ClaimDTO>> rolesClaims);
    }
}
