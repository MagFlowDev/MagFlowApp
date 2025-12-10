using MagFlow.Domain.Core;
using MagFlow.EF.Seeds.Company;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.Core
{
    public static class CoreDbSeeder
    {
        public static void Seed(CoreDbContext context)
        {
            try
            {
                var type = typeof(ICoreSeeder);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
                List<ICoreSeeder> seeders = new List<ICoreSeeder>();
                foreach (var iseeder in types)
                {
                    try
                    {
                        var seeder = (ICoreSeeder)Activator.CreateInstance(iseeder);
                        if(seeder != null )
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
                        seeder?.Seed(context);
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

        public static async Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken)
        {
            try
            {
                var type = typeof(ICoreSeeder);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
                List<ICoreSeeder> seeders = new List<ICoreSeeder>();
                foreach (var iseeder in types)
                {
                    try
                    {
                        var seeder = (ICoreSeeder)Activator.CreateInstance(iseeder);
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
