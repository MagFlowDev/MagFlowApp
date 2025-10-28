using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public interface ICompanyDbContextFactory : IDbContextFactory<CompanyDbContext>
    {
        CompanyDbContext CreateDbContext(string connectionString);

        Task<CompanyDbContext> CreateDbContextAsync(string connectionString, CancellationToken cancellationToken = default)
        => Task.FromResult(CreateDbContext(connectionString));
    }
}
