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

        public CompanyDbContext CreateDbContext(string connectionString, int companyNumber)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new CompanyDbContext(optionsBuilder.Options, companyNumber);
        }

        public CompanyDbContext CreateDbContext()
        {
            var connectionString = _companyContext.ConnectionString;
            var companyNumber = _companyContext.CompanyNumber;
            if(string.IsNullOrEmpty(connectionString))
                throw new NotSupportedException("CompanyDb context requires to provide ConnectionString");
            if (!companyNumber.HasValue || companyNumber <= 0)
                throw new NotSupportedException("CompanyDb context required to provice CompanyNumber");
            return CreateDbContext(connectionString, companyNumber.Value);
        }
    }
}
