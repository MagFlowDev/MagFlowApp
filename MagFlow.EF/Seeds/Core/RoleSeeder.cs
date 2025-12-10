using MagFlow.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagFlow.Shared.Constants;
using MagFlow.Shared.Models.Enumerators;

namespace MagFlow.EF.Seeds.Core
{
    public class RoleSeeder : ICoreSeeder
    {
        public int Step => 2;

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
                    Id = AppRole.SuperAdmin.Id,
                    Name = AppRole.SuperAdmin.Name,
                    NormalizedName = AppRole.SuperAdmin.Name.ToUpper()
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

            foreach (var role in Enumeration<Guid>.GetAll<AppRole>().Where(x => x.Id != AppRole.SuperAdmin.Id && x.Name != AppRole.SuperAdmin.Name))
            {
                var dbRole = await context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == role.Name.ToUpper());
                if (dbRole == null)
                {
                    dbRole = new ApplicationRole
                    {
                        Id = role.Id,
                        Name = role.Name,
                        NormalizedName = role.Name.ToUpper()
                    };
                    await context.ApplicationRoles.AddAsync(dbRole);
                    seed = true;
                }
            }
            
            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
