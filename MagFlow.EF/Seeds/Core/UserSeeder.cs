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
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;
            var adminUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@MAGFLOW.PL");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Id = new Guid("3594E385-7A0A-41E7-5B87-08DE1BD4DEF0"),
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
                    PasswordHash = "AQAAAAIAAYagAAAAENh7VI3HK8k9lJGj9Pu+j7BbKIxrHDpO4lJZkfPBfWBBF0kO+V2y4S5eNTNLIFjJDg==",
                    SecurityStamp = "RAI3U53B22DPXJN3GUCLAAPS2HWIYYFP",
                    ConcurrencyStamp = "111922fa-b149-4843-83c7-580ae89b42c4",
                };
                await context.ApplicationUsers.AddAsync(adminUser);
                seed = true;
            }

            if(seed)
                await context.SaveChangesAsync();
        }
    }
}
