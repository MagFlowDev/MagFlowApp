using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Shared.Models.FormModels;

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
                DbName = company.DbName,
                TaxNumber = company.TaxNumber,
                CreatedAt = company.CreatedAt,
                IsActive = company.IsActive,
                Address = company.Address,
                LogoData = company.Logo?.ImageData,
                LogoContentType = company.Logo?.ContentType,
                CompanySettings = company.CompanySettings?.ToDTO(),
                CompanyModules = company.Modules?.ToDTO()
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
                DbName = companyDTO.DbName,
                NormalizedName = companyDTO.Name.ToUpper(),
                TaxNumber = companyDTO.TaxNumber,
                ConnectionString = StringExtensions.GetCompanyConnectionString(companyDTO.DbName) ?? string.Empty,
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

        public static Company ToEntity(this CompanyFormModel model)
        {
            return new Company()
            {
                Id = Guid.NewGuid(),
                Name = model.GeneralInformation.Name,
                NormalizedName = model.GeneralInformation.Name.Normalize().ToUpper(),
                DbName = model.GeneralInformation.DbName,
                TaxNumber = model.GeneralInformation.TaxNumber,
                ConnectionString = StringExtensions.GetCompanyConnectionString(model.GeneralInformation.DbName) ?? string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Address = new Address()
                {
                    Line1 = model.GeneralInformation.Address.Line1,
                    City = model.GeneralInformation.Address.City,
                    ZipCode = model.GeneralInformation.Address.ZipCode,
                    Country = model.GeneralInformation.Address.Country
                },
                CompanySettings = new CompanySettings()
                {
                    Email = model.ContactData.Email,
                    PhoneNumber = model.ContactData.PhoneNumber,
                    Website = model.ContactData.Website
                }
            };
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
