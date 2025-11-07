using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.MultiTenancy
{
    public interface ITenantProvider
    {
        Task<string?> GetTenantConnectionString(string userEmail);

        Task<string?> GetTenantConnectionString(Guid companyId);
    }
}
