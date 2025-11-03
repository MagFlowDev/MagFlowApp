using MagFlow.Domain.Core;
using MagFlow.EF;
using MagFlow.EF.Seeds.Company;
using MagFlow.EF.Seeds.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Helpers
{
    public static class DbInitializer
    {
        public static async Task Initialize(CoreDbContext coreDbContext, 
            RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            ILoggerFactory? loggerFactory)
        {
            ILogger? logger = loggerFactory?.CreateLogger($"{nameof(MagFlow)}.{nameof(BLL)}.{nameof(Helpers)}.{nameof(DbInitializer)}");
            try
            {
                logger?.LogInformation("Database initializing process started...");

                await coreDbContext.Database.MigrateAsync();
                await CoreDbSeeder.SeedAsync(coreDbContext, CancellationToken.None);
                await MigrateCompanies(coreDbContext, logger);

                logger?.LogInformation("Database initializing proces successfully finished.");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.Message);
            }
        }

        private static async Task MigrateCompanies(CoreDbContext coreDbContext, ILogger? logger)
        {

            var companies = await coreDbContext.Companies.ToListAsync();
            foreach(var company in companies)
            {
                try
                {
                    using (var companyDbContext = new CompanyDbContext(company.ConnectionString))
                    {
                        await companyDbContext.Database.MigrateAsync();
                        await CompanyDbSeeder.SeedAsync(companyDbContext, CancellationToken.None);
                    }
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, $"Company {company.Name} migration error");
                }
            }
        }
    }
}
