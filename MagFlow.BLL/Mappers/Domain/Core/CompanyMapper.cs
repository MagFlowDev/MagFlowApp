using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Domain.Core
{
    public static class CompanyMapper
    {
        public static CompanyDTO ToDTO(this Company company)
        {
            return new CompanyDTO()
            {
                Id = company.Id,
                Name = company.Name,
                NIP = company.NIP
            };
        }

        public static List<CompanyDTO> ToDTO(this IEnumerable<Company> companies)
        {
            return companies.Select(x => ToDTO(x)).ToList();
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
                NIP = companyDTO.NIP,
                ConnectionString = StringExtensions.GetCompanyConnectionString(companyDTO.Name) ?? string.Empty,
                IsActive = isActive,
                CreatedAt = createdAt.Value,
            };
        }

        public static Company Validate(this Company company, CompanyDTO companyDTO)
        {
            company.Name = !string.IsNullOrWhiteSpace(companyDTO.Name) ? companyDTO.Name : company.Name;
            company.NIP = company.NIP;
            return company;
        }
    }
}
