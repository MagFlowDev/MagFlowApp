using MagFlow.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.MultiTenancy
{
    public class TenantProvider : ITenantProvider
    {
        private readonly ICoreDbContextFactory _coreContextFactory;

        public TenantProvider(ICoreDbContextFactory coreContextFactory)
        {
            _coreContextFactory = coreContextFactory;
        }

        public async Task<(string? connectionString, int? companyNumber)> GetTenantInfo(string userEmail)
        {
            try
            {
                using(var context = _coreContextFactory.CreateDbContext())
                {
                    var normalizedEmail = userEmail.ToUpper();
                    var user = await context.ApplicationUsers
                        .Include(c => c.DefaultCompany)
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

                    var connectionString = user?.DefaultCompany?.ConnectionString;
                    var companyNumber = user?.DefaultCompany?.CompanyNumber;

                    return (connectionString, companyNumber);
                }
            }
            catch(Exception ex)
            {
                return (null, null);
            }
        }

        public async Task<(string? connectionString, int? companyNumber)> GetTenantInfo(Guid companyId)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var company = await context.Companies.FirstOrDefaultAsync(i => i.Id == companyId);

                    var connectionString = company?.ConnectionString;
                    var companyNumber = company?.CompanyNumber;

                    return (connectionString, companyNumber);
                }
            }
            catch (Exception ex)
            {
                return (null, null);
            }
        }
    }
}
