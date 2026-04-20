using MagFlow.BLL.Security.Requirements;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MagFlow.BLL.Security.Handlers
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

        public PermissionHandler(IUserRepository userRepository, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = context.User;
            if (user?.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission) == true)
            {
                context.Succeed(requirement);
                return;
            }

            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Fail();
                return;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                context.Fail();
                return;
            }

            if (!string.IsNullOrEmpty(requirement.Permission))
            {
  
                if (requirement.Permission.Contains('.'))
                {
                    var parts = requirement.Permission.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && Enum.TryParse<PermissionFlags>(parts[1], ignoreCase: true, out var neededFlag))
                    {
                        var moduleCode = parts[0];
                        var claim = user.FindFirst($"perms:{moduleCode}")?.Value;
                        if (!string.IsNullOrEmpty(claim) && long.TryParse(claim, out var mask))
                        {
                            if (((PermissionFlags)mask & neededFlag) == neededFlag)
                            {
                                context.Succeed(requirement);
                                return;
                            }
                        }
                    }
                }
                else
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
            }

            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;
            var cacheKey = $"user_permissions_{userIdClaim}";
            if(!_cache.TryGetValue(cacheKey, out Dictionary<string, long> moduleMasks))
            {
                try
                {
                    var userRoleClaims = new List<MagFlow.Domain.CompanyScope.Claim>();
                    if (!string.IsNullOrEmpty(userRole))
                    {
                        userRoleClaims = await _userRepository.GetRoleClaims(userRole);
                    }
                    else if (!Guid.TryParse(userIdClaim, out var uid))
                    {
                        userRoleClaims = await _userRepository.GetUserClaims(uid);
                    }
                    else
                    {
                        userRoleClaims = await _userRepository.GetUserClaims(uid);
                    }
                    var permissionStrings = ExtractPermissionNames(userRoleClaims);
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
                }
                catch
                {
                    moduleMasks = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
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

        public static string[] ExtractPermissionNamesStatic(List<MagFlow.Domain.CompanyScope.Claim>? userClaims) => ExtractPermissionNames(userClaims);

        private static string[] ExtractPermissionNames(List<MagFlow.Domain.CompanyScope.Claim>? userEntity)
        {
            if (userEntity == null)
                return Array.Empty<string>();

            return userEntity.Select(x => x.Policy).ToArray();

            var userType = userEntity.GetType();
            var permProp = userType.GetProperty("Permissions");
            if (permProp != null)
            {
                var permVal = permProp.GetValue(userEntity);
                if (permVal is System.Collections.Generic.IEnumerable<string> permsStr)
                    return permsStr.ToArray();

                if (permVal is System.Collections.IEnumerable permsObj)
                {
                    var list = permsObj.Cast<object?>()
                        .Select(o => o?.GetType().GetProperty("Name")?.GetValue(o)?.ToString())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray()!;
                    return list;
                }
            }

            var rolesProp = userType.GetProperty("Roles");
            if (rolesProp != null)
            {
                var rolesVal = rolesProp.GetValue(userEntity);
                if (rolesVal is System.Collections.IEnumerable rolesEnum)
                {
                    var collected = rolesEnum.Cast<object?>()
                        .SelectMany(r =>
                        {
                            if (r == null) return Array.Empty<string>();
                            var roleType = r.GetType();
                            var rolePermsProp = roleType.GetProperty("Permissions");
                            if (rolePermsProp == null) return Array.Empty<string>();
                            var rp = rolePermsProp.GetValue(r);
                            if (rp is System.Collections.IEnumerable rpEnum)
                            {
                                return rpEnum.Cast<object?>()
                                    .Select(x => x?.GetType().GetProperty("Name")?.GetValue(x)?.ToString())
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .Select(s => s!);
                            }
                            return Array.Empty<string>();
                        })
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                    return collected;
                }
            }

            return Array.Empty<string>();
        }
    }
}
