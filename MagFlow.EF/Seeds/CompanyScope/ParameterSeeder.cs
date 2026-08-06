using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public class ParameterSeeder : ICompanySeeder
    {
        public int Step => 5;

        public void Seed(CompanyDbContext context, string companyName)
        {
            Task.Run(async () => await SeedAsync(context, companyName, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, string companyName, CancellationToken cancellationToken)
        {
            if (string.Compare(companyName, "test", true) != 0)
                return;

            bool seed = false;

            var units = await context.Units.ToListAsync();

            var parametersNames = parameters.Select(x => x.Name);
            var existingParameters = await context.CustomParameters
                .Where(x => parametersNames.Contains(x.Name))
                .Select(x => x.Name)
                .ToListAsync();
            var notExistingParametersNames = parametersNames.Except(existingParameters);
            var notExistingParameters = parameters.Where(x => notExistingParametersNames.Contains(x.Name));
            foreach (var parameter in notExistingParameters)
            {
                var unit = units.FirstOrDefault(x => x.Name == parameter.Unit?.Name);
                parameter.Unit = null;
                parameter.UnitId = unit?.Id;
                await context.CustomParameters.AddAsync(parameter);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }

        List<CustomParameter> parameters = new List<CustomParameter>()
        {
            new CustomParameter(){ Name = "Szerokość", ValueType = Enums.ValueType.Decimal, Unit = new Unit(){ Name = "centymetr" }},
            new CustomParameter(){ Name = "Długość", ValueType = Enums.ValueType.Decimal, Unit = new Unit(){ Name = "centymetr" }},
            new CustomParameter(){ Name = "Wysokość", ValueType = Enums.ValueType.Decimal, Unit = new Unit(){ Name = "centymetr" }},
        };
    }
}
