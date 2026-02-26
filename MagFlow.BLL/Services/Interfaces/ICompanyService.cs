using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDTO?> GetCurrentCompany();
        Task<Enums.Result> CreateCompany(CompanyDTO companyDTO);
        Task<List<ModuleDTO>?> GetCompanyModules(Guid companyId);

        Task<Enums.Result> UpdateCompany(CompanyDTO companyDTO);
        Task<Enums.Result> UpdateCompanySettings(Guid companyId, CompanySettingsDTO companySettingsDTO);
        Task<Enums.Result> UpdateCompanyLogo(Guid companyId, byte[] data, string contentType = "image/jpg");
    }
}
