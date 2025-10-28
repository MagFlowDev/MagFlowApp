using MagFlow.Domain.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class CompanyDbContext : DbContext
    {
        public DbSet<Contractor> Contractors { get; set; }

        public CompanyDbContext(string connectionString) : base(BuildOptions(connectionString))
        {
        }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));
        }

        private static DbContextOptions<CompanyDbContext> BuildOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }
}
