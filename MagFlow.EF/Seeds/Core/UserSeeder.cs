using MagFlow.Domain.Core;
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
            var adminUser = context.Set<ApplicationUser>().FirstOrDefault(u => u.NormalizedEmail == "ADMIN@MAGFLOW.PL");
            if (adminUser == null)
            {

            }
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            
        }
    }
}
