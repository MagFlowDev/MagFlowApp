using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class FactoryRoleStore : RoleStore<ApplicationRole, CoreDbContext, Guid, ApplicationUserRole, ApplicationRoleClaim>
    {
        private readonly IDbContextFactory<CoreDbContext> _contextFactory;

        public FactoryRoleStore(IDbContextFactory<CoreDbContext> contextFactory, IdentityErrorDescriber? describer = null) 
            : base(contextFactory.CreateDbContext(), describer)
        {
            _contextFactory = contextFactory;
        }
    }
}
