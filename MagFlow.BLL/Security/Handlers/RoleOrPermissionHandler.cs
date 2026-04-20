using MagFlow.BLL.Security.Requirements;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using MagFlow.Shared.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.BLL.Security.Handlers
{
    public class RoleOrPermissionHandler : AuthorizationHandler<RoleOrPermissionRequirement>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

        public RoleOrPermissionHandler(IUserRepository userRepository, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleOrPermissionRequirement requirement)
        {
            var user = context.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Fail();
                return;
            }

            // TODO: Zrobic uprawnienia jako bitmask zamiast np User.Edit, User.Delete, User.Add niech bedzie np. User.10 gdzie 10 bedzie pobrane z enuma
            //[Flags]
            //public enum Permissions
            //{
            //    None = 0,
            //    Read = 1,    // 0001
            //    Write = 2,   // 0010
            //    Delete = 4,  // 0100
            //    Admin = 8    // 1000
            //}

            //if (!string.IsNullOrEmpty(requirement.Role) && user.IsInRole(requirement.Role))
            //{
            //    context.Succeed(requirement);
            //    return;
            //}

            if (!string.IsNullOrEmpty(requirement.Permission) &&
                user.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
                return;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return;
            }

            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            var cacheKey = $"user_permissions_{userId}";
            if (!_cache.TryGetValue(cacheKey, out string[] permissions))
            {
                var uid = Guid.TryParse(userId, out var g) ? g : Guid.Empty;
                var userRoleClaims = new List<MagFlow.Domain.CompanyScope.Claim>();
                if (!string.IsNullOrEmpty(userRole))
                    userRoleClaims = await _userRepository.GetRoleClaims(userRole);
                else
                    userRoleClaims = await _userRepository.GetUserClaims(uid);
                permissions = PermissionHandler.ExtractPermissionNamesStatic(userRoleClaims);
                _cache.Set(cacheKey, permissions, CacheTtl);
            }

            if (!string.IsNullOrEmpty(requirement.Permission) &&
                permissions.Contains(requirement.Permission, StringComparer.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}
