using MagFlow.Domain.CoreScope;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class FactoryUserStore : UserStore<ApplicationUser, ApplicationRole, CoreDbContext, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>, IUserSecurityStampStore<ApplicationUser>
    {
        private readonly IDbContextFactory<CoreDbContext> _contextFactory;

        public FactoryUserStore(IDbContextFactory<CoreDbContext> contextFactory, IdentityErrorDescriber? describer = null) 
            : base(contextFactory.CreateDbContext(), describer)
        {
            _contextFactory = contextFactory;
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }
    }
}
