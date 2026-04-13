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
            if (context.User?.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission) == true)
            {
                context.Succeed(requirement);
                return;
            }

            if (context.User?.Identity?.IsAuthenticated != true)
            {
                context.Fail();
                return;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                context.Fail();
                return;
            }

            var cacheKey = $"user_permissions_{userIdClaim}";
            if(!_cache.TryGetValue(cacheKey, out string[] permissions))
            {
                try
                {
                    if (!Guid.TryParse(userIdClaim, out var uid))
                    {
                        var userEntity = await _userRepository.GetByIdAsync(userIdClaim);
                        permissions = ExtractPermissionNames(userEntity);
                    }
                    else
                    {
                        var userEntity = await _userRepository.GetByIdAsync(uid);
                        permissions = ExtractPermissionNames(userEntity);
                    }
                }
                catch
                {
                    permissions = Array.Empty<string>();
                }

                _cache.Set(cacheKey, permissions, CacheTtl);
            }

            if (permissions != null && permissions.Contains(requirement.Permission, StringComparer.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }

        public static string[] ExtractPermissionNamesStatic(object? userEntity) => ExtractPermissionNames(userEntity);

        private static string[] ExtractPermissionNames(object? userEntity)
        {
            if (userEntity == null)
                return Array.Empty<string>();

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
