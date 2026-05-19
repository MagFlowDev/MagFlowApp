using MagFlow.Domain.CompanyScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public class UnitSeeder : ICompanySeeder
    {
        public int Step => 1;

        public void Seed(CompanyDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;

            var unitsSymbols = units.Select(x => x.Symbol);
            var existingsSymbols = await context.Units
                .Where(x => unitsSymbols.Contains(x.Symbol))
                .Select(x => x.Symbol)
                .ToListAsync();
            var notExistingSymbols = unitsSymbols.Except(existingsSymbols);
            var notExistingUnits = units.Where(x => notExistingSymbols.Contains(x.Symbol));
            foreach(var unit in notExistingUnits)
            {
                await context.Units.AddAsync(unit);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }

        List<Unit> units = new List<Unit>()
        {
            new Unit { Symbol = "kg", Name = "kilogram", RelatedUnits = new List<Unit>()
            {
                new Unit { Symbol = "g", Name = "gram", ParentUnitConversionRate = 1000 },
                new Unit { Symbol = "mg", Name = "miligram", ParentUnitConversionRate = 1000000 },
            }},
            new Unit { Symbol = "l", Name = "litr", RelatedUnits = new List<Unit>()
            {
                new Unit { Symbol = "ml", Name = "mililitr", ParentUnitConversionRate = 1000 },
            }},
            new Unit { Symbol = "szt", Name = "sztuka" }
        };
    }
}
