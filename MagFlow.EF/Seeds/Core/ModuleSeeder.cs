using MagFlow.Domain.Core;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.Seeds.Core
{
    public class ModuleSeeder : ICoreSeeder
    {
        public void Seed(CoreDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;

            foreach(var module in Enumeration.GetAll<ModuleType>())
            {
                var dbModule = await context.Modules.FirstOrDefaultAsync(m => m.Name.Equals(module.Name));
                if (dbModule == null)
                {
                    dbModule = new Module()
                    {
                        Id = Guid.NewGuid(),
                        Name = module.Name,
                    };
                    await context.ApplicationRoles.AddAsync(dbRole);
                    seed = true;
                }
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
