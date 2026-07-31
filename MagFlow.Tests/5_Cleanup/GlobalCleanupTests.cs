using MagFlow.Tests._0_Setup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Tests._5_Cleanup
{
    [Collection("DatabaseSequentialTests")]
    [TestCaseOrderer(typeof(PriorityOrderer))]
    public class GlobalCleanupTests
    {
        private readonly TestDatabaseFixture _fixture;

        public GlobalCleanupTests(TestDatabaseFixture fixture) => _fixture = fixture;

        [Fact, Priority(1)]
        public async Task DropCompanyDatabase_ShouldRemoveIsolatedStorage()
        {
            using var companyContext = _fixture.CreateCompanyContext();
            bool isDropped = await companyContext.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
            Microsoft.Data.SqlClient.SqlConnection.ClearAllPools();
            bool exists = await companyContext.Database.CanConnectAsync(TestContext.Current.CancellationToken);
            Assert.False(exists);
        }

        [Fact, Priority(2)]
        public async Task DropCoreDatabase_ShouldRemoveCentralStorage()
        {
            using var coreContext = _fixture.CreateCoreContext();
            bool isDropped = await coreContext.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
            Microsoft.Data.SqlClient.SqlConnection.ClearAllPools();
            Assert.True(isDropped);
        }
    }
}
