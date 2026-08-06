using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public class ProductSeeder : ICompanySeeder
    {
        public int Step => 8;

        public void Seed(CompanyDbContext context, string companyName)
        {
            Task.Run(async () => await SeedAsync(context, companyName, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, string companyName, CancellationToken cancellationToken)
        {
            if (string.Compare(companyName, "test", true) != 0)
                return;
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == "test@magflow.com");
            if (user == null)
                return;

            bool seed = false;

            var units = await context.Units.ToListAsync();
            var categories = await context.ProductCategories.ToListAsync();
            var types = await context.ProductTypes.ToListAsync();

            var productsNames = products.Select(x => x.Name);
            var existingProducts = await context.Products
                .Where(x => productsNames.Contains(x.Name))
                .Select(x => x.Name)
                .ToListAsync();
            var notExistingProductNames = productsNames.Except(existingProducts);
            var notExistingProducts = products.Where(x => notExistingProductNames.Contains(x.Name));
            foreach (var product in notExistingProducts)
            {
                var type = types.FirstOrDefault(x => x.Name == product.Type?.Name);
                var category = categories.FirstOrDefault(x => x.Name == product.Category?.Name);
                var unit = units.FirstOrDefault(x => x.Name == product.Unit?.Name);

                product.Type = null;
                product.Category = null;
                product.Unit = null;

                product.TypeId = type?.Id ?? 0;
                product.CategoryId = category?.Id ?? 0;
                product.UnitId = unit?.Id ?? 0;
                product.CreatedById = user?.Id ?? Guid.Empty;

                await context.Products.AddAsync(product);
                seed = true;
            }

            if (seed)
                await context.SaveChangesAsync();
        }

        List<Product> products = new List<Product>()
        {
            new Product()
            { 
                Name = "Ryza Gloss V1", 
                IsActive = true, 
                Status = Enums.EntityStatus.Active, 
                Unit = new Unit(){ Name = "sztuka" }, 
                Category = new ProductCategory(){ Name = "Ryza" },  
                Type = new ProductType(){ Name = "Gloss" }
            }
        };
    }
}
