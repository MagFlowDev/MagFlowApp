using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public class ProductCategorySeeder : ICompanySeeder
    {
        public int Step => 6;

        public void Seed(CompanyDbContext context, string companyName)
        {
            Task.Run(async () => await SeedAsync(context, companyName, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, string companyName, CancellationToken cancellationToken)
        {
            if (string.Compare(companyName, "test", true) != 0)
                return;

            bool seed = false;

            var categoriesNames = categories.Select(x => x.Name);
            var existingCategories = await context.ProductCategories
                .Where(x => categoriesNames.Contains(x.Name))
                .Select(x => x.Name)
                .ToListAsync();
            var notExistingCategoriesNames = categoriesNames.Except(existingCategories);
            var notExistingCategories = categories.Where(x => notExistingCategoriesNames.Contains(x.Name));
            foreach (var category in notExistingCategories)
            {
                await context.ProductCategories.AddAsync(category);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }

        List<ProductCategory> categories = new List<ProductCategory>()
        {
            new ProductCategory(){ Name = "Papier rolowy", IsActive = true, IsBasic = true },
            new ProductCategory(){ Name = "Papier arkuszowy", IsActive = true, IsBasic = true },
            new ProductCategory(){ Name = "Ryza", IsActive = true, IsBasic = false },
        };
    }
}
