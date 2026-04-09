using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
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
        Task<CompanyDTO?> GetCompany(Guid companyId);
        Task<List<ModuleDTO>?> GetCompanyModules(Guid companyId);
        Task<List<ModuleDTO>?> GetAllModules();
        Task<List<DefaultWorkingHourDTO>?> GetDefaultWorkingHours();
        Task<QueryResponse<UserDTO>> GetUsers(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<CompanyDTO>> GetCompanies(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);

        Task<Enums.Result> CreateCompany(CompanyFormModel model);

        Task<Enums.Result> SelectCompany(CompanyDTO companyDTO);
        Task<Enums.Result> AdminSelectCompany(CompanyDTO companyDTO);
        Task<Enums.Result> UpdateCompany(CompanyDTO companyDTO);
        Task<Enums.Result> UpdateCompanySettings(Guid companyId, CompanySettingsDTO companySettingsDTO);
        Task<Enums.Result> UpdateCompanyLogo(Guid companyId, byte[] data, string contentType = "image/jpg");
        Task<Enums.Result> UpdateDefaultWorkingHours(List<DefaultWorkingHourDTO> defaultWorkingHourDTOs);
        Task<Enums.Result> UpdateWorkDays(List<WorkDayDTO> workDayDTOs);

        Task<Enums.Result> UpdateModulesLicense(CompanyDTO companyDTO, List<Guid> moduleIds, DateTime expireDate, bool activate = false);
        Task<Enums.Result> UpdateModulesLicense(CompanyDTO companyDTO, List<Guid> moduleIds, Enums.LongTimePeriod timePeriod, bool activate = false);

        Task<Enums.Result> AddCompanyUser(UserFormModel model);

        Task<Enums.Result> RemoveCompanyLogo(Guid companyId);

        Task<Enums.Result> DeleteCompany(CompanyDTO companyDTO);

        Task<Enums.Result> SendExtendModuleRequest(Guid companyId, Guid moduleId, string user, string message);
        Task<Enums.Result> SendAskForModuleOfferRequest(Guid companyId, Guid moduleId, string user, string message);
    }
}
