using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.Core
{
    public interface ICoreSeeder
    {
        void Seed(CoreDbContext context);
        Task SeedAsync(CoreDbContext context, CancellationToken cancellationToken);
    }
}
