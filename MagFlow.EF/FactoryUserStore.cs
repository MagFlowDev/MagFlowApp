using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class FactoryUserStore : UserStore<ApplicationUser, ApplicationRole, CoreDbContext, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>
    {
        private readonly IDbContextFactory<CoreDbContext> _contextFactory;

        public FactoryUserStore(IDbContextFactory<CoreDbContext> contextFactory, IdentityErrorDescriber? describer = null) 
            : base(contextFactory.CreateDbContext(), describer)
        {
            _contextFactory = contextFactory;
        }
    }
}
