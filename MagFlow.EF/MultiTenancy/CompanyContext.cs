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

        public int? CompanyNumber { get; private set; }

        public CompanyContext(ITenantProvider tenantProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _tenantProvider = tenantProvider;
            
            SetCompanyContext(httpContextAccessor);
        }

        public async Task SetCompanyContext(string userEmail)
        {
            var tenantInfo = await _tenantProvider.GetTenantInfo(userEmail);
            ConnectionString = tenantInfo.connectionString;
            CompanyNumber = tenantInfo.companyNumber;
        }

        private void SetCompanyContext(IHttpContextAccessor httpContextAccessor)
        {
            var companyIdClaim = httpContextAccessor?.HttpContext?.User?.FindFirst(Claims.CompanyClaim)?.Value;
            if (!Guid.TryParse(companyIdClaim, out var companyId))
                return;
            try
            {
                var tenantInfo = Task.Run(async () =>
                {
                    return await _tenantProvider.GetTenantInfo(companyId).ConfigureAwait(false);
                }).Result;
                ConnectionString = tenantInfo.connectionString;
                CompanyNumber = tenantInfo.companyNumber;
            }
            catch(Exception ex)
            {

            }
        }
    }
}
