using MagFlow.Domain.CoreScope;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.CoreScope
{
    public class UserSeeder : ICoreSeeder
    {
        public int Step => 3;

        public void Seed(CoreDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            bool seedTestUsers = true;

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
                    CreatedAt = now,
                    LastLogin = now,
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
            if(seedTestUsers)
            {
                await SeedTestUsers(context, cancellationToken, "TEST", 100);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }



        private async Task SeedTestUsers(CoreDbContext context, CancellationToken cancellationToken, string company, int count)
        {
            var now = DateTime.UtcNow;
            for (int i = 1; i < count; i++)
            {
                var user = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == $"TEST_{i}@MAGFLOW.COM");
                if (user == null)
                {
                    var testCompany = await context.Companies.FirstOrDefaultAsync(u => u.NormalizedName == company);
                    var password = new PasswordHasher<ApplicationUser>();
                    user = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Test",
                        LastName = "Magflow",
                        CreatedAt = now,
                        LastLogin = now,
                        DefaultCompanyId = testCompany?.Id,
                        IsActive = true,
                        UserName = $"test_{i}@magflow.com",
                        NormalizedUserName = $"TEST_{i}@MAGFLOW.COM",
                        Email = $"test_{i}@magflow.com",
                        NormalizedEmail = $"TEST_{i}@MAGFLOW.COM",
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
                    user.PasswordHash = password.HashPassword(user, "Password1!");

                    await context.ApplicationUsers.AddAsync(user);
                    var foremanRole = await context.ApplicationRoles.FirstOrDefaultAsync(r => r.NormalizedName == "FOREMAN");
                    if (foremanRole != null)
                    {
                        ApplicationUserRole foreman = new ApplicationUserRole { RoleId = foremanRole.Id, UserId = user.Id };
                        await context.UserRoles.AddAsync(foreman);
                    }
                    if (testCompany != null)
                    {
                        CompanyUser companyUser = new CompanyUser() { CompanyId = testCompany.Id, UserId = user.Id, AssignedAt = now };
                        await context.CompanyUsers.AddAsync(companyUser);
                        using (var companyContext = new CompanyDbContext(testCompany.ConnectionString))
                        {
                            var tempUser = new Domain.CompanyScope.User()
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email
                            };
                            companyContext.Users.Add(tempUser);
                            companyContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
