using MagFlow.Domain.CoreScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Tests._0_Setup
{
    [Collection("DatabaseSequentialTests")]
    [TestCaseOrderer(typeof(PriorityOrderer))]
    public class DatabaseDeploymentTests
    {
        private readonly TestDatabaseFixture _fixture;

        public DatabaseDeploymentTests(TestDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Priority(1)]
        public async Task CreateCoreDatabase_ShouldEstablishSchema()
        {
                        using var coreContext = _fixture.CreateCoreContext();
            bool isCreated = await coreContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
            Assert.True(isCreated);
        }

        [Fact, Priority(2)]
        public async Task InsertCompany_ShouldGenerateSequentialNumber()
        {
            using var coreContext = _fixture.CreateCoreContext();
            var company = new Company
            {
                Id = _fixture.TestCompanyGuid,
                Name = "Test Company",
                DbName = "Test",
                NormalizedName = "TEST",
                TaxNumber = "6700123056",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Address = new Shared.Models.Address()
                {
                    Line1 = "Testowa 1",
                    City = "Krakow",
                    Country = "PL",
                    ZipCode = "30-123"
                },
                ConnectionString = _fixture.CompanyConnectionString
            };

            coreContext.Companies.Add(company);
            await coreContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            _fixture.TestCompanyNumber = company.CompanyNumber;
            Assert.NotEqual(0, _fixture.TestCompanyNumber);
        }


        [Fact, Priority(3)]
        public async Task CreateCompanyDatabase_ShouldBuildIsolatedSchema()
        {
            if (_fixture.TestCompanyNumber == 0)
            {
                Assert.Skip("Skipped: Step InsertCompany has failed.");
            }

            using var companyContext = _fixture.CreateCompanyContext();
            await companyContext.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
            bool isCreated = await companyContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
            Assert.True(isCreated);
        }

        [Fact, Priority(4)]
        public async Task InsertAdminUser_ShouldLinkToCompanyGuid()
        {
            using var coreContext = _fixture.CreateCoreContext();
            var now = DateTime.UtcNow;
            var user = new ApplicationUser
            {
                Email = _fixture.TestAdminUserEmail,
                FirstName = "Jan",
                LastName = "Kowalski",
                CreatedAt = now,
                LastLogin = now,
                IsActive = true,
                DefaultCompanyId = _fixture.TestCompanyGuid
            };
            coreContext.Users.Add(user);
            await coreContext.SaveChangesAsync(TestContext.Current.CancellationToken);
            var company = coreContext.Companies.Include(x => x.Users).ThenInclude(y => y.User).FirstOrDefault(u => u.Id == _fixture.TestCompanyGuid);

            var companyUser = new CompanyUser()
            {
                CompanyId = company.Id,
                UserId = user.Id,
                AssignedAt = now,
            };
            
            coreContext.CompanyUsers.Add(companyUser);
            await coreContext.SaveChangesAsync(TestContext.Current.CancellationToken);
            company = coreContext.Companies.Include(x => x.Users).ThenInclude(y => y.User).FirstOrDefault(u => u.Id == _fixture.TestCompanyGuid);


            var dbUser = coreContext.Users.FirstOrDefault(u => u.Email == _fixture.TestAdminUserEmail);
            var dbCompanyUser = company?.Users?.FirstOrDefault(u => u.User?.Email == _fixture.TestAdminUserEmail);

            Assert.Collection(new object?[] { dbUser, dbCompanyUser },
                u => Assert.NotNull(u),
                cu => Assert.NotNull(cu));

            _fixture.IsSetupSuccessful = true;
        }
    }
}
