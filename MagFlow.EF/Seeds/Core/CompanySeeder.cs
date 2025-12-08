using MagFlow.Domain.Core;
using MagFlow.EF.Seeds.Company;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Helpers.Generators;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MagFlow.EF.Seeds.Core
{
    public class CompanySeeder : ICoreSeeder
    {
        public void Seed(CoreDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            List<string> seededDbConnectionStrings = new List<string>();
            bool seed = false;

            var now = DateTime.UtcNow;
            var testCompany = await context.Companies.FirstOrDefaultAsync(c => c.NormalizedName.Equals("TEST"));
            if (testCompany == null)
            {
                testCompany = new Domain.Core.Company()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    NormalizedName = "TEST",
                    CreatedAt = now,
                    IsActive = true,
                    ConnectionString = StringExtensions.GetCompanyConnectionString("Test") ?? "",
                    NIP = ""
                };
                await context.Companies.AddAsync(testCompany);
                seed = true;
                seededDbConnectionStrings.Add(testCompany.ConnectionString);
            }
            var demoCompany = await context.Companies.FirstOrDefaultAsync(c => c.NormalizedName.Equals("DEMO"));
            if (demoCompany == null)
            {
                demoCompany = new Domain.Core.Company()
                {
                    Id = Guid.NewGuid(),
                    Name = "Demo",
                    NormalizedName = "DEMO",
                    CreatedAt = now,
                    IsActive = true,
                    ConnectionString = StringExtensions.GetCompanyConnectionString("Demo") ?? "",
                    NIP = ""
                };
                await context.Companies.AddAsync(demoCompany);
                var adminUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@MAGFLOW.COM");
                if (adminUser != null)
                {
                    CompanyUser companyUser = new CompanyUser() { CompanyId = demoCompany.Id, UserId = adminUser.Id, AssignedAt = now };
                    await context.CompanyUsers.AddAsync(companyUser);
                    if(adminUser.DefaultCompanyId == null)
                    {
                        adminUser.DefaultCompanyId = demoCompany.Id;
                        context.ApplicationUsers.Update(adminUser);
                    }
                }
                seed = true;
                seededDbConnectionStrings.Add(demoCompany.ConnectionString);
            }
            else
            {
                var adminUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@MAGFLOW.COM");
                if (adminUser != null)
                {
                    if (!await context.CompanyUsers.AnyAsync(x => x.UserId == adminUser.Id && x.CompanyId == demoCompany.Id))
                    {
                        CompanyUser companyUser = new CompanyUser() { CompanyId = demoCompany.Id, UserId = adminUser.Id, AssignedAt = now };
                        await context.CompanyUsers.AddAsync(companyUser);
                        seed = true;
                    }
                    if(adminUser.DefaultCompanyId == null)
                    {
                        adminUser.DefaultCompanyId = demoCompany.Id;
                        context.ApplicationUsers.Update(adminUser);
                        seed = true;
                    }
                }
            }

            if (seed)
                await context.SaveChangesAsync();

            if(seededDbConnectionStrings.Any())
            {
                foreach(var connectionString in seededDbConnectionStrings)
                {
                    try
                    {
                        using (var companyDbContext = new CompanyDbContext(connectionString))
                        {
                            await companyDbContext.Database.MigrateAsync();
                            await CompanyDbSeeder.SeedAsync(companyDbContext, CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }
    }
}
