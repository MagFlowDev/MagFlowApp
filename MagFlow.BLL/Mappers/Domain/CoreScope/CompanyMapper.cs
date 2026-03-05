using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;

namespace MagFlow.BLL.Mappers.Domain.CoreScope
{
    public static class CompanyMapper
    {
        public static CompanyDTO ToDTO(this Company company)
        {
            return new CompanyDTO()
            {
                Id = company.Id,
                Name = company.Name,
                TaxNumber = company.TaxNumber,
                CreatedAt = company.CreatedAt,
                Address = company.Address,
                LogoData = company.Logo?.ImageData,
                LogoContentType = company.Logo?.ContentType,
                CompanySettings = ToDTO(company.CompanySettings),
                CompanyModules = ToDTO(company.Modules),
            };
        }

        public static List<CompanyDTO> ToDTO(this IEnumerable<Company> companies)
        {
            return companies.Select(x => ToDTO(x)).ToList();
        }

        public static CompanySettingsDTO ToDTO(this CompanySettings? companySettings)
        {
            return new CompanySettingsDTO()
            {
                Email = companySettings?.Email,
                PhoneNumber = companySettings?.PhoneNumber,
                Website = companySettings?.Website,
            };
        }

        public static CompanyModuleDTO ToDTO(this CompanyModule companyModule)
        {
            return new CompanyModuleDTO()
            {
                ModuleId = companyModule.ModuleId,
                AssignedAt = companyModule.AssignedAt,
                EnabledFrom = companyModule.EnabledFrom,
                EnabledTo = companyModule.EnabledTo,
                IsActive = companyModule.IsActive,
                Code = companyModule.Module?.Code ?? string.Empty,
                Name = companyModule.Module?.Name ?? string.Empty,
                Description = companyModule.Module?.Description,
                Type = ModuleType.GetModuleType(companyModule.Module?.Name)
            };
        }

        public static List<CompanyModuleDTO> ToDTO(this ICollection<CompanyModule> companyModules)
        {
            var groupedModules = companyModules
                .GroupBy(x => x.ModuleId)
                .Select(g => g.OrderByDescending(x => x.EnabledTo).FirstOrDefault());
            return groupedModules?.Where(x => x != null).Select(x => x!.ToDTO()).ToList() ?? new List<CompanyModuleDTO>();
        }



        public static Company ToEntity(this CompanyDTO companyDTO, bool isActive = true, DateTime? createdAt = null)
        {
            if (!createdAt.HasValue)
                createdAt = DateTime.UtcNow;
            return new Company()
            {
                Id = companyDTO.Id ?? Guid.NewGuid(),
                Name = companyDTO.Name,
                NormalizedName = companyDTO.Name.ToUpper(),
                TaxNumber = companyDTO.TaxNumber,
                ConnectionString = StringExtensions.GetCompanyConnectionString(companyDTO.Name) ?? string.Empty,
                IsActive = isActive,
                CreatedAt = createdAt.Value,
                Address = companyDTO.Address ?? new Address()
            };
        }

        public static CompanySettings ToEntity(this CompanySettingsDTO companySettingsDTO, Guid companyId)
        {
            return new CompanySettings()
            {
                CompanyId = companyId,
                Email = companySettingsDTO?.Email,
                PhoneNumber = companySettingsDTO?.PhoneNumber,
                Website = companySettingsDTO?.Website
            };
        }

        public static CompanySettings ToEntity(this CompanySettingsDTO userSettingsDTO, CompanySettings actualSettings)
        {
            actualSettings.Email = userSettingsDTO.Email ?? actualSettings.Email;
            actualSettings.PhoneNumber = userSettingsDTO.PhoneNumber ?? actualSettings.PhoneNumber;
            actualSettings.Website = userSettingsDTO.Website ?? actualSettings.Website;
            return actualSettings;
        }



        public static Company Validate(this Company company, CompanyDTO companyDTO)
        {
            company.Name = !string.IsNullOrWhiteSpace(companyDTO.Name) ? companyDTO.Name : company.Name;
            company.TaxNumber = !string.IsNullOrWhiteSpace(companyDTO.TaxNumber) ? companyDTO.TaxNumber : company.TaxNumber;
            company.Address = companyDTO.Address ?? company.Address;
            return company;
        }
    }
}
