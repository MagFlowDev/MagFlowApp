using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.MultiTenancy
{
    public interface ITenantProvider
    {
        Task<(string? connectionString, int? companyNumber)> GetTenantInfo(string userEmail);

        Task<(string? connectionString, int? companyNumber)> GetTenantInfo(Guid companyId);
    }
}
