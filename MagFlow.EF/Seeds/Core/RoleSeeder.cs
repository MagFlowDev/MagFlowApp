using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.Seeds.Core
{
    public class RoleSeeder : ICoreSeeder
    {
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
                
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
