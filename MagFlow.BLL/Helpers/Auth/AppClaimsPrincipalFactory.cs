using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.Models.Auth;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MagFlow.BLL.Helpers.Auth
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            IOptions<IdentityOptions> options,
            IUserRepository userRepository,
            IMemoryCache cache) 
            : base(userManager, roleManager, options)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (user == null)
                return identity;

            var cacheKey = $"user_permissions_{user.Id}";
            if (!_cache.TryGetValue(cacheKey, out Dictionary<string, long> moduleMasks))
            {
                var userRoleClaims = await _userRepository.GetUserClaims(user.Id);
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

            foreach (var kv in moduleMasks)
            {
                var claimType = $"perms:{kv.Key}";
                if (identity.HasClaim(c => string.Equals(c.Type, claimType, StringComparison.OrdinalIgnoreCase)))
                {
                    var existingClaim = identity.FindFirst(c => string.Equals(c.Type, claimType, StringComparison.OrdinalIgnoreCase));
                    identity.RemoveClaim(existingClaim);
                }
                identity.AddClaim(new Claim(claimType, kv.Value.ToString()));
            }

            if (!identity.HasClaim(c => c.Type == Claims.CompanyClaim))
                identity.AddClaim(new Claim(Claims.CompanyClaim, user.DefaultCompanyId?.ToString() ?? Guid.Empty.ToString()));

            return identity;
        }
    }
}
