using MagFlow.BLL.Mappers.Domain.CoreScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IRoleRepository _roleRepository;

        private readonly ILogger<RoleService> _logger;

        public RoleService(ICompanyRepository companyRepository,
            IRoleRepository roleRepository,
            ILogger<RoleService> logger)
        {
            _companyRepository = companyRepository;
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<List<ClaimDTO>> GetAllClaims()
        {
            var claims = await _roleRepository.GetAllClaims();
            var claimsDTOs = claims.ToDTO();
            return claimsDTOs;
        }

        public async Task<Dictionary<Guid, List<ClaimDTO>>> GetRolesClaims(List<Guid> rolesIds)
        {
            var roleClaims = await _roleRepository.GetRolesClaims(rolesIds);
            var roleClaimsDTOs = roleClaims.ToDictionary(x => x.Key, x => x.Value.ToDTO());
            return roleClaimsDTOs;
        }

        public async Task<Dictionary<Guid, List<ClaimDTO>>> GetRolesClaims(List<string> rolesNames)
        {
            var roleClaims = await _roleRepository.GetRolesClaims(rolesNames);
            var roleClaimsDTOs = roleClaims.ToDictionary(x => x.Key, x => x.Value.ToDTO());
            return roleClaimsDTOs;
        }

        public async Task<Enums.Result> UpdateRolesClaims(Dictionary<Guid, List<ClaimDTO>> rolesClaims)
        {
            var claimsToAdd = new List<RoleClaim>();
            var claimsToUpdate = new List<RoleClaim>();
            var claimsToDelete = new List<RoleClaim>();

            var allRoles = Enumeration<Guid>.GetAll<AppRole>().ToList();
            foreach (var roleClaims in rolesClaims)
            {
                var roleId = roleClaims.Key;
                var roleName = allRoles.FirstOrDefault(x => x.Id == roleId)?.Name;
                if (string.IsNullOrEmpty(roleName))
                    continue;

                var toAdd = roleClaims.Value.Where(x => !x.ToDelete && x.ToAdd).ToEntity()
                    .Select(x => new RoleClaim() { RoleId = roleId, RoleName = roleName, ClaimId = x.Id });
                var toUpdate = roleClaims.Value.Where(x => !x.ToDelete && !x.ToAdd).ToEntity()
                    .Select(x => new RoleClaim() { RoleId = roleId, RoleName = roleName, ClaimId = x.Id });
                var toDelete = roleClaims.Value.Where(x => x.ToDelete).ToEntity()
                    .Select(x => new RoleClaim() { RoleId = roleId, RoleName = roleName, ClaimId = x.Id });
                claimsToAdd.AddRange(toAdd);
                claimsToUpdate.AddRange(toUpdate);
                claimsToDelete.AddRange(toDelete);
            }

            //var result = await _roleRepository.UpdateRolesClaims(claimsToUpdate);
            //if (result != Enums.Result.Success)
            //    return result;
            var result = await _roleRepository.DeleteRolesClaims(claimsToDelete);
            if (result != Enums.Result.Success)
                return result;
            result = await _roleRepository.AddRolesClaims(claimsToAdd);
            return result;
        }
    }
}
