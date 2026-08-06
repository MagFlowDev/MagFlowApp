using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public interface ICompanySeeder
    {
        int Step { get; }

        void Seed(CompanyDbContext context, string companyName);
        Task SeedAsync(CompanyDbContext context, string companyName, CancellationToken cancellationToken);
    }
}
