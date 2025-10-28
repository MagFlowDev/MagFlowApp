using MagFlow.Shared.Models.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class CoreDbContextFactory : ICoreDbContextFactory
    {
        public CoreDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            optionsBuilder.UseSqlServer(AppSettings.ConnectionStrings.CoreDb);
            return new CoreDbContext(optionsBuilder.Options);
        }
    }
}
