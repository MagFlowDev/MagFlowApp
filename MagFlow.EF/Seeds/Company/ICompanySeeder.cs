using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.Company
{
    public interface ICompanySeeder
    {
        void Seed(CompanyDbContext context);
        Task SeedAsync(CompanyDbContext context, CancellationToken cancellationToken);
    }
}
