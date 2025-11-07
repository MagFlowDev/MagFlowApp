using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.MultiTenancy
{
    public class CompanyContext : ICompanyContext
    {
        private readonly ITenantProvider _tenantProvider;

        public string? ConnectionString { get; private set; }

        public CompanyContext(ITenantProvider tenantProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _tenantProvider = tenantProvider;
            
            SetCompanyContext(httpContextAccessor);
        }

        public async Task SetCompanyContext(string userEmail)
        {
            ConnectionString = await _tenantProvider.GetTenantConnectionString(userEmail);
        }

        private void SetCompanyContext(IHttpContextAccessor httpContextAccessor)
        {
            var companyIdClaim = httpContextAccessor?.HttpContext?.User?.FindFirst(Claims.CompanyClaim)?.Value;
            if (!Guid.TryParse(companyIdClaim, out var companyId))
                return;
            try
            {
                ConnectionString = Task.Run(async () =>
                {
                    return await _tenantProvider.GetTenantConnectionString(companyId).ConfigureAwait(false);
                }).Result;
            }
            catch(Exception ex)
            {

            }
        }
    }
}
