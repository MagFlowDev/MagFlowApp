using MagFlow.Domain.Core;
using MagFlow.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Helpers
{
    public static class DbSeeder
    {
        public static async Task Seed(CoreDbContext coreDbContext, 
            RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            ILoggerFactory? loggerFactory)
        {
            ILogger? logger = loggerFactory?.CreateLogger($"{nameof(MagFlow)}.{nameof(BLL)}.{nameof(Helpers)}.{nameof(DbSeeder)}");
            try
            {
                logger?.LogInformation("Database seed process started...");

                await coreDbContext.Database.MigrateAsync();
                await MigrateCompanies(coreDbContext, logger);

                logger?.LogInformation("Database seed proces successfully finished.");
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
