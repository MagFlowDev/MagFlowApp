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

        public async Task<string?> GetTenantConnectionString(string userEmail)
        {
            try
            {
                using(var context = _coreContextFactory.CreateDbContext())
                {
                    var normalizedEmail = userEmail.ToUpper();
                    var user = await context.ApplicationUsers
                        .Include(c => c.DefaultCompany)
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

                    return user?.DefaultCompany?.ConnectionString;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<string?> GetTenantConnectionString(Guid companyId)
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var company = await context.Companies.FirstOrDefaultAsync(i => i.Id == companyId);

                    return company?.ConnectionString;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
