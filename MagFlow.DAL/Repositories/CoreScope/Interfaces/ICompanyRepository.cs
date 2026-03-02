using MagFlow.Domain.CoreScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface ICompanyRepository : IRepository<MagFlow.Domain.CoreScope.Company>
    {
        Task<List<CompanyModule>?> GetCompanyModules(Guid companyId);

        Task<Enums.Result> UpdateLogoAsync(Guid companyId, byte[] data, string contentType);
        Task<Enums.Result> RemoveLogoAsync(Guid companyId);
        Task<Enums.Result> UpdateSettingsAsync(CompanySettings companySettings);
    }
}
