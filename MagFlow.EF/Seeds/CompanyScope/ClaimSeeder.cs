using MagFlow.Domain.CompanyScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public class ClaimSeeder : ICompanySeeder
    {
        public int Step => 4;

        public void Seed(CompanyDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;

            var claimDictionary = typeof(MagFlow.Shared.Constants.Policies.Claims)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .ToDictionary(
                    field => field.Name,
                    field => field.GetValue(null)?.ToString()
                );
            if (claimDictionary == null)
                return;

            var claimsNames = claimDictionary.Keys;
            var existingClaims = await context.Claims
                .Where(x => claimsNames.Contains(x.Name))
                .Select(x => x.Name)
                .ToListAsync();
            var notExistingClaims = claimsNames.Except(existingClaims);
            var claimsToAdd = claimDictionary.Where(x => notExistingClaims.Contains(x.Key));

            foreach (var claimToAdd in claimsToAdd)
            {
                if(string.IsNullOrEmpty(claimToAdd.Value))
                    continue;
                var claim = new Claim()
                {
                    Id = Guid.NewGuid(),
                    Name = claimToAdd.Key,
                    Policy = claimToAdd.Value
                };
                await context.Claims.AddAsync(claim);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
