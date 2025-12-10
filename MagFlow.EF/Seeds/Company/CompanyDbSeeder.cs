using MagFlow.EF.Seeds.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.Company
{
    public static class CompanyDbSeeder
    {
        public static void Seed(CompanyDbContext context)
        {
            try
            {
                var type = typeof(ICompanySeeder);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
                List<ICompanySeeder> seeders = new List<ICompanySeeder>();
                foreach (var iseeder in types)
                {
                    try
                    {
                        var seeder = (ICompanySeeder)Activator.CreateInstance(iseeder);
                        if(seeder != null)
                            seeders.Add(seeder);   
                    }
                    catch(Exception ex)
                    {

                    }
                }
                seeders = seeders.OrderBy(s => s.Step).ToList();
                foreach (var seeder in seeders)
                {
                    try
                    {
                        seeder?.Seed(context);
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task SeedAsync(CompanyDbContext context, CancellationToken cancellationToken)
        {
            try
            {
                var type = typeof(ICompanySeeder);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
                List<ICompanySeeder> seeders = new List<ICompanySeeder>();
                foreach (var iseeder in types)
                {
                    try
                    {
                        var seeder = (ICompanySeeder)Activator.CreateInstance(iseeder);
                        if (seeder != null)
                            seeders.Add(seeder);
                    }
                    catch (Exception ex)
                    {

                    }   
                }
                seeders = seeders.OrderBy(s => s.Step).ToList();
                foreach (var seeder in seeders)
                {
                    try
                    {
                        if (seeder != null)
                            await seeder.SeedAsync(context, cancellationToken);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
