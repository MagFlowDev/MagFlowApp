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
        public int Step => 1;

        public void Seed(CoreDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;
            var now = DateTime.UtcNow;
            var adminUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@MAGFLOW.COM");
            if (adminUser == null)
            {
                var demoCompany = await context.Companies.FirstOrDefaultAsync(u => u.NormalizedName == "DEMO");
                var password = new PasswordHasher<ApplicationUser>();
                adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "Magflow",
                    CreatedAt = DateTime.UtcNow,
                    DefaultCompanyId = demoCompany?.Id,
                    IsActive = true,
                    UserName = "admin@magflow.com",
                    NormalizedUserName = "ADMIN@MAGFLOW.COM",
                    Email = "admin@magflow.com",
                    NormalizedEmail = "ADMIN@MAGFLOW.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserSettings = new ApplicationUserSettings
                    {
                        Language = Shared.Models.Enums.Language.Polish,
                        ThemeMode = Shared.Models.Enums.ThemeMode.LightMode,
                        DecimalSeparator = Shared.Models.Enums.DecimalSeparator.Comma,
                        DateFormat = Shared.Models.Enums.DateFormat.DD_MM_RRRR_DOTS,
                        TimeFormat = Shared.Models.Enums.TimeFormat.HH_MM_24H,
                        TimeZone = Shared.Models.Enums.TimeZone.Europe_Warsaw
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
                if (demoCompany != null)
                {
                    CompanyUser companyUser = new CompanyUser() { CompanyId = demoCompany.Id, UserId = adminUser.Id, AssignedAt = now };
                    await context.CompanyUsers.AddAsync(companyUser);
                }
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
