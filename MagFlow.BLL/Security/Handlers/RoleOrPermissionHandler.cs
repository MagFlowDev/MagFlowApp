using MagFlow.BLL.Security.Requirements;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Models.Auth;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

            if (!string.IsNullOrEmpty(requirement.Permission) && requirement.Permission.Contains('.'))
            {
                var parts = requirement.Permission.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && Enum.TryParse<PermissionFlags>(parts[1], ignoreCase: true, out var neededFlag))
                {
                    var moduleCode = parts[0];
                    if (user.HasModulePermission(moduleCode, neededFlag))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(requirement.Permission))
            {
                if (Enum.TryParse<PermissionFlags>(requirement.Permission, ignoreCase: true, out var neededAny))
                {
                    var anyMatch = user.Claims
                        .Where(c => c.Type.StartsWith("perms:", StringComparison.OrdinalIgnoreCase))
                        .Select(c => long.TryParse(c.Value, out var v) ? (PermissionFlags)v : PermissionFlags.None)
                        .Any(mask => (mask & neededAny) == neededAny);

                    if (anyMatch)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return;
            }

            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            var cacheKey = $"user_permissions_{userId}";
            if (!_cache.TryGetValue(cacheKey, out Dictionary<string, long> moduleMasks))
            {
                var uid = Guid.TryParse(userId, out var g) ? g : Guid.Empty;
                var userRoleClaims = new List<MagFlow.Domain.CompanyScope.Claim>();
                if (!string.IsNullOrEmpty(userRole))
                    userRoleClaims = await _userRepository.GetRoleClaims(userRole);
                else
                    userRoleClaims = await _userRepository.GetUserClaims(uid);
                var permissionStrings = MagFlow.BLL.Security.Handlers.PermissionHandler.ExtractPermissionNamesStatic(userRoleClaims);

                moduleMasks = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

                foreach (var perm in permissionStrings)
                {
                    var parts = perm.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 2)
                        continue;

                    var moduleCode = parts[0].Trim();
                    var permName = parts[1].Trim();

                    if (Enum.TryParse<PermissionFlags>(permName, ignoreCase: true, out var flag))
                    {
                        if (!moduleMasks.TryGetValue(moduleCode, out var current))
                            current = 0;
                        current |= (long)flag;
                        moduleMasks[moduleCode] = current;
                    }
                }
                _cache.Set(cacheKey, moduleMasks, CacheTtl);
            }

            if (!string.IsNullOrEmpty(requirement.Permission))
            {
                if (requirement.Permission.Contains('.'))
                {
                    var parts = requirement.Permission.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && Enum.TryParse<PermissionFlags>(parts[1], ignoreCase: true, out var neededFlag))
                    {
                        var moduleCode = parts[0];
                        if (moduleMasks.TryGetValue(moduleCode, out var mask) && ((PermissionFlags)mask & neededFlag) == neededFlag)
                        {
                            context.Succeed(requirement);
                            return;
                        }
                    }
                }
                else
                {
                    if (Enum.TryParse<PermissionFlags>(requirement.Permission, ignoreCase: true, out var neededAny))
                    {
                        foreach (var kv in moduleMasks)
                        {
                            if (((PermissionFlags)kv.Value & neededAny) == neededAny)
                            {
                                context.Succeed(requirement);
                                return;
                            }
                        }
                    }
                }
            }

            context.Fail();
        }
    }
}
