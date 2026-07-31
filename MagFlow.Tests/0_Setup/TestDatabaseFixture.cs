using MagFlow.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Tests._0_Setup
{
    public class TestDatabaseFixture : IAsyncLifetime
    {
        public string CoreConnectionString { get; } = "Server=.\\SQLExpress;Database=MagFlow_Automation_CoreDb;User Id=sa;Password=Password1!;TrustServerCertificate=True";
        public string CompanyConnectionString { get; } = "Server=.\\SQLExpress;Database=MagFlow_Automation_CompanyDb_Test;User Id=sa;Password=Password1!;TrustServerCertificate=True";

        public Guid TestCompanyGuid { get; private set; }
        public string TestAdminUserEmail { get; } = "admin@magflow.test";
        public string TestUserEmail { get; } = "user@magflow.test";
        public int TestCompanyNumber { get; set; }

        public bool IsSetupSuccessful { get; set; } = false;

        public CoreDbContext CreateCoreContext()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>().UseSqlServer(CoreConnectionString).Options;
            return new CoreDbContext(options);
        }

        public CompanyDbContext CreateCompanyContext()
        {
            var options = new DbContextOptionsBuilder<CompanyDbContext>().UseSqlServer(CompanyConnectionString).Options;
            return new CompanyDbContext(options, TestCompanyNumber);
        }

        public async ValueTask InitializeAsync()
        {
            TestCompanyGuid = Guid.NewGuid();

            using var coreContext = CreateCoreContext();
            await coreContext.Database.EnsureDeletedAsync();
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
