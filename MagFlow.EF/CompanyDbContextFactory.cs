using MagFlow.Shared.Models.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class CompanyDbContextFactory : ICompanyDbContextFactory
    {
        public CompanyDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new CompanyDbContext(optionsBuilder.Options);
        }

        public CompanyDbContext CreateDbContext()
        {
            throw new NotSupportedException("CompanyDb context requires to provide ConnectionString");
        }
    }
}
