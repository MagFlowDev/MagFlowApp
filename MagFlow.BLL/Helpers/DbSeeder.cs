using MagFlow.Domain.Core;
using MagFlow.EF;
using Microsoft.AspNetCore.Identity;
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
            logger?.LogInformation("Database seed process started...");
        }
    }
}
