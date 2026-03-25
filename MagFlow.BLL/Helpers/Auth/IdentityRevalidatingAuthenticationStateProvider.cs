using MagFlow.Domain.CoreScope;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MagFlow.BLL.Helpers.Auth
{
    public class IdentityRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<IdentityOptions> _options;
        private readonly ILogger<IdentityRevalidatingAuthenticationStateProvider> _logger;

        public IdentityRevalidatingAuthenticationStateProvider(ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> options,
            ILogger<IdentityRevalidatingAuthenticationStateProvider> logger) : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = options;
            _logger = logger;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(5);

        public async Task RevalidateNowAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            var valid = await ValidateAuthenticationStateAsync(authState, CancellationToken.None);
            if(!valid)
            {
                var emptyUser = new ClaimsPrincipal(new ClaimsIdentity());
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(emptyUser)));
            }
        }

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user is null)
            {
                return false;
            }
            else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(_options.Value.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }
    }
}
