using MagFlow.EF.MultiTenancy;
using MagFlow.Shared.Models.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class CompanyDbContextFactory : ICompanyDbContextFactory
    {
        private readonly ICompanyContext _companyContext;

        public CompanyDbContextFactory(ICompanyContext companyContext)
        {
            _companyContext = companyContext;
        }

        public CompanyDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new CompanyDbContext(optionsBuilder.Options);
        }

        public CompanyDbContext CreateDbContext()
        {
            var connectionString = _companyContext.ConnectionString;
            if(string.IsNullOrEmpty(connectionString))
                throw new NotSupportedException("CompanyDb context requires to provide ConnectionString");
            return CreateDbContext(connectionString);
        }
    }
}
