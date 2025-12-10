using MagFlow.Domain.Company;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.Seeds.Company
{
    public class UnitConversionSeeder : ICompanySeeder
    {
        public int Step => 2;

        public void Seed(CompanyDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;

            var unitsSymbols = conversions.Select(c => c.ToUnitSymbol).Union(conversions.Select(c => c.FromUnitSymbol));
            var existingsUnits = await context.Units
                .Where(x => unitsSymbols.Contains(x.Symbol))
                .ToListAsync();
            var existingSymbols = existingsUnits.Select(u => u.Symbol);
            conversions = conversions.Where(c => existingSymbols.Contains(c.ToUnitSymbol) && existingSymbols.Contains(c.FromUnitSymbol)).ToList();

            foreach(var conversion in conversions)
            {
                var fromUnit = existingsUnits.FirstOrDefault(u => u.Symbol == conversion.FromUnitSymbol);
                var toUnit = existingsUnits.FirstOrDefault(u => u.Symbol == conversion.ToUnitSymbol);
                if (fromUnit == null || toUnit == null || fromUnit.Id == toUnit.Id)
                    continue;

                if (await context.UnitConversions.AnyAsync(uc => uc.FromUnitId == fromUnit.Id && uc.ToUnitId == toUnit.Id))
                    continue;

                var dbConversion = new UnitConversion()
                {
                    FromUnitId = fromUnit.Id,
                    ToUnitId = toUnit.Id,
                    ConversionRate = conversion.ConversionRate
                };
                await context.UnitConversions.AddAsync(dbConversion);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }

        List<TempUnitConversion> conversions = new List<TempUnitConversion>()
        {
            new TempUnitConversion() { FromUnitSymbol = "kg", ToUnitSymbol = "g", ConversionRate = 1000m },
            new TempUnitConversion() { FromUnitSymbol = "g", ToUnitSymbol = "kg", ConversionRate = 0.001m },

            new TempUnitConversion() { FromUnitSymbol = "g", ToUnitSymbol = "mg", ConversionRate = 1000m },
            new TempUnitConversion() { FromUnitSymbol = "mg", ToUnitSymbol = "g", ConversionRate = 0.001m },

            new TempUnitConversion() { FromUnitSymbol = "l", ToUnitSymbol = "ml", ConversionRate = 1000m },
            new TempUnitConversion() { FromUnitSymbol = "ml", ToUnitSymbol = "l", ConversionRate = 0.001m },

        };

        class TempUnitConversion
        {
            public string FromUnitSymbol { get; set; }
            public string ToUnitSymbol { get; set; }
            public decimal ConversionRate { get; set; }
        }
    }
}
