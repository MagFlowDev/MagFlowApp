using MagFlow.BLL.Security.Requirements;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

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

            if (!string.IsNullOrEmpty(requirement.Role) && user.IsInRole(requirement.Role))
            {
                context.Succeed(requirement);
                return;
            }

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

            var cacheKey = $"user_permissions_{userId}";
            if (!_cache.TryGetValue(cacheKey, out string[] permissions))
            {
                var uid = Guid.TryParse(userId, out var g) ? g : Guid.Empty;
                var userEntity = await _userRepository.GetByIdAsync(uid);
                permissions = PermissionHandler.ExtractPermissionNamesStatic(userEntity);
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
