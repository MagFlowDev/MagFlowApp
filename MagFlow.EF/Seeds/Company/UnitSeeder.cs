using MagFlow.Domain.Company;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.Seeds.Company
{
    public class UnitSeeder : ICompanySeeder
    {
        Dictionary<string, string> units = new Dictionary<string, string>()
        {
            { "Kg", "Kilogram" },
            { "g", "gram" },
            { "mg", "miligram" },
            { "l", "litr" },
            { "ml", "mililitr" },
            { "szt", "sztuka" }
        };

        public void Seed(CompanyDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;

            var unitsSymbols = units.Keys;
            var existingsSymbols = await context.Units
                .Where(x => unitsSymbols.Contains(x.Symbol))
                .Select(x => x.Symbol)
                .ToListAsync();
            var notExistingSymbols = unitsSymbols.Except(existingsSymbols);
            var notExistingUnits = units.Where(x => notExistingSymbols.Contains(x.Key));
            foreach(var unit in notExistingUnits)
            {
                var dbUnit = new Unit()
                {
                    Symbol = unit.Key,
                    Name = unit.Value
                };
                await context.Units.AddAsync(dbUnit);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
