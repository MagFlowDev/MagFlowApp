using MagFlow.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.Seeds.Core
{
    public class RoleSeeder : ICoreSeeder
    {
        public void Seed(CoreDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;
            var superAdminRole = await context.ApplicationRoles.FirstOrDefaultAsync(r => r.NormalizedName == "SUPERADMIN");
            if (superAdminRole == null)
            {
                superAdminRole = new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                };
                await context.ApplicationRoles.AddAsync(superAdminRole);
                var adminUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@MAGFLOW.COM");
                if (adminUser != null)
                {
                    ApplicationUserRole superAdmin = new ApplicationUserRole { RoleId = superAdminRole.Id, UserId = adminUser.Id };
                    await context.UserRoles.AddAsync(superAdmin);
                }
                seed = true;
            }
            else
            {
                var adminUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@MAGFLOW.COM");
                if(adminUser != null)
                {
                    if(!await context.UserRoles.AnyAsync(x => x.UserId == adminUser.Id && x.RoleId == superAdminRole.Id))
                    {
                        ApplicationUserRole superAdmin = new ApplicationUserRole { RoleId = superAdminRole.Id, UserId = adminUser.Id };
                        await context.UserRoles.AddAsync(superAdmin);
                        seed = true;
                    }
                }
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
