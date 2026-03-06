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
                CompanyModules = company.Modules.ToDTO()
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
