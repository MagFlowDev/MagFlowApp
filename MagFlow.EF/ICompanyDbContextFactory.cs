using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public interface ICompanyDbContextFactory : IDbContextFactory<CompanyDbContext>
    {
        CompanyDbContext CreateDbContext(string connectionString, int companyNumber);

        Task<CompanyDbContext> CreateDbContextAsync(string connectionString, int companyNumber, CancellationToken cancellationToken = default)
            => Task.FromResult(CreateDbContext(connectionString, companyNumber));
    }
}
