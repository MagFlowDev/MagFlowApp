using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public class ProductTypeSeeder : ICompanySeeder
    {
        public int Step => 7;

        public void Seed(CompanyDbContext context, string companyName)
        {
            Task.Run(async () => await SeedAsync(context, companyName, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, string companyName, CancellationToken cancellationToken)
        {
            if (string.Compare(companyName, "test", true) != 0)
                return;

            bool seed = false;

            var categories = await context.ProductCategories.ToListAsync();

            var typesNames = types.Select(x => x.Name);
            var existingTypes= await context.ProductTypes
                .Where(x => typesNames.Contains(x.Name))
                .Select(x => x.Name)
                .ToListAsync();
            var notExistingTypesNames = typesNames.Except(existingTypes);
            var notExistingTypes = types.Where(x => notExistingTypesNames.Contains(x.Name));
            foreach (var type in notExistingTypes)
            {
                var category = categories.FirstOrDefault(x => x.Name == type.Category?.Name);
                type.Category = null;
                type.CategoryId = category?.Id ?? 0;
                await context.ProductTypes.AddAsync(type);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }

        List<ProductType> types = new List<ProductType>()
        {
            new ProductType(){ Name = "Matt", IsActive = true, Category = new ProductCategory(){ Name = "Papier rolowy" }},
            new ProductType(){ Name = "Offset", IsActive = true, Category = new ProductCategory(){ Name = "Papier rolowy" }},
            new ProductType(){ Name = "Silk", IsActive = true, Category = new ProductCategory(){ Name = "Papier arkuszowy" }},
            new ProductType(){ Name = "Gloss", IsActive = true, Category = new ProductCategory(){ Name = "Papier arkuszowy" }},
            new ProductType(){ Name = "Gloss", IsActive = true, Category = new ProductCategory(){ Name = "Ryza" }},
        };
    }
}
