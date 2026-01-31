using MagFlow.Domain.Core;
using MagFlow.Shared.Helpers.Generators;
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
        public int Step => 4;

        public void Seed(CoreDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;

            var now = DateTime.UtcNow;
            var testCompany = await context.Companies
                .Include(x => x.Modules).ThenInclude(y => y.Module)
                .FirstOrDefaultAsync(c => c.NormalizedName.Equals("TEST"));
            var demoCompany = await context.Companies
                .Include(x => x.Modules).ThenInclude(y => y.Module)
                .FirstOrDefaultAsync(c => c.NormalizedName.Equals("DEMO"));
            foreach(var module in Enumeration<Guid>.GetAll<ModuleType>())
            {
                var dbModule = await context.Modules.FirstOrDefaultAsync(m => m.Name.Equals(module.Name));
                if (dbModule == null)
                {
                    dbModule = new Module()
                    {
                        Id = module.Id,
                        Name = module.Name,
                        Description = ModuleType.GetModuleTypeDescription(ModuleType.GetModuleType(module.Name)),
                        CreatedAt = now,
                        Code = CodeGenerator.Module_GenerateCode(module.Name),
                        IsActive = true
                    };
                    await context.Modules.AddAsync(dbModule);
                    seed = true;
                }

                if (testCompany != null && !testCompany.Modules.Any(x => x.ModuleId == module.Id))
                {
                    CompanyModule companyModule = new CompanyModule()
                    {
                        CompanyId = testCompany.Id,
                        ModuleId = module.Id,
                        AssignedAt = now,
                        EnabledFrom = now,
                        EnabledTo = DateTime.MaxValue,
                        IsActive = true,
                    };
                    await context.CompanyModules.AddAsync(companyModule);
                    seed = true;
                }
                
                if (demoCompany != null && !demoCompany.Modules.Any(x => x.ModuleId == module.Id))
                {
                    CompanyModule companyModule = new CompanyModule()
                    {
                        CompanyId = demoCompany.Id,
                        ModuleId = module.Id,
                        AssignedAt = now,
                        EnabledFrom = now,
                        EnabledTo = DateTime.MaxValue,
                        IsActive = true,
                    };
                    await context.CompanyModules.AddAsync(companyModule);
                    seed = true;
                }
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
