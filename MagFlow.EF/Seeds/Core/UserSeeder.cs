using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.Core
{
    public class UserSeeder : ICoreSeeder
    {
        public void Seed(CoreDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;
            var adminUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@MAGFLOW.COM");
            if (adminUser == null)
            {
                var password = new PasswordHasher<ApplicationUser>();
                adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "Magflow",
                    CreatedAt = DateTime.UtcNow,
                    DefaultCompanyId = null,
                    IsActive = true,
                    UserName = "admin@magflow.com",
                    NormalizedUserName = "ADMIN@MAGFLOW.COM",
                    Email = "admin@magflow.com",
                    NormalizedEmail = "ADMIN@MAGFLOW.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserSettings = new ApplicationUserSettings
                    {
                        Language = Shared.Models.Enums.Language.Polish
                    }
                };
                adminUser.PasswordHash = password.HashPassword(adminUser, "Password1!");
                
                await context.ApplicationUsers.AddAsync(adminUser);
                var superAdminRole = await context.ApplicationRoles.FirstOrDefaultAsync(r => r.NormalizedName == "SUPERADMIN");
                if (superAdminRole != null)
                {
                    ApplicationUserRole superAdmin = new ApplicationUserRole { RoleId = superAdminRole.Id, UserId = adminUser.Id };
                    await context.UserRoles.AddAsync(superAdmin);
                }
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
