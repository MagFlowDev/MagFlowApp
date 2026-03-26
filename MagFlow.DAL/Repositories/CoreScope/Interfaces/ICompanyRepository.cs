using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface ICompanyRepository : IRepository<Company, CoreDbContext>
    {
        Task<List<CompanyModule>?> GetCompanyModules(Guid companyId);

        Task<Enums.Result> UpdateLogoAsync(Guid companyId, byte[] data, string contentType);
        Task<Enums.Result> RemoveLogoAsync(Guid companyId);
        Task<Enums.Result> UpdateSettingsAsync(CompanySettings companySettings);

        List<Guid>? RemoveAllUsersFromCompany(Company entity);
        Task<List<Guid>?> RemoveAllUsersFromCompanyAsync(Company entity);
    }
}
