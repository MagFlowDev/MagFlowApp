using MagFlow.Domain.Core;
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
                foreach (var iseeder in types)
                {
                    var seeder = (ICoreSeeder)Activator.CreateInstance(iseeder);
                    seeder?.Seed(context);
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
                foreach (var iseeder in types)
                {
                    var seeder = (ICoreSeeder)Activator.CreateInstance(iseeder);
                    if(seeder != null)
                        await seeder.SeedAsync(context, cancellationToken);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
