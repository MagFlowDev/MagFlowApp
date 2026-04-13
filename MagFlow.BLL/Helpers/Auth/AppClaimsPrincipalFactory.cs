using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.Models.Auth;
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
            if (!_cache.TryGetValue(cacheKey, out string[] permissions))
            {
                var userEntity = await _userRepository.GetByIdAsync(user.Id);
                permissions = MagFlow.BLL.Security.Handlers.PermissionHandler.ExtractPermissionNamesStatic(userEntity);
                _cache.Set(cacheKey, permissions, CacheTtl);
            }

            foreach (var p in permissions.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                identity.AddClaim(new Claim("permission", p));
            }

            if (!identity.HasClaim(c => c.Type == Claims.CompanyClaim))
                identity.AddClaim(new Claim(Claims.CompanyClaim, user.DefaultCompanyId?.ToString() ?? Guid.Empty.ToString()));

            return identity;
        }
    }
}
