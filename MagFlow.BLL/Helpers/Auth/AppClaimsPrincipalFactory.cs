using MagFlow.Domain.CoreScope;
using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MagFlow.BLL.Helpers.Auth
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            IOptions<IdentityOptions> options) 
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (!identity.HasClaim(c => c.Type == Claims.CompanyClaim))
                identity.AddClaim(new Claim(Claims.CompanyClaim, user.DefaultCompanyId?.ToString() ?? Guid.Empty.ToString()));

            return identity;
        }
    }
}
