using MagFlow.BLL.Mappers.Domain.Core;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MagFlow.BLL.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(ICompanyRepository companyRepository,
            ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<List<ModuleDTO>?> GetCompanyModules(Guid companyId)
        {
            var modules = await _companyRepository.GetCompanyModules(companyId);
            if(modules == null)
                return null;
            return modules
                .Where(x => x.Module != null)
                .Select(x => x.Module!)
                .ToDTO();
        }
        
        public async Task<Enums.Result> CreateCompany(CompanyDTO companyDTO)
        {
            Company company = companyDTO.ToEntity();
            if (string.IsNullOrEmpty(company.Name) || string.IsNullOrEmpty(company.ConnectionString))
                return Enums.Result.Error;

            var result = await _companyRepository.AddAsync(company);
            return result;
        }

        public async Task<Enums.Result> UpdateCompany(CompanyDTO companyDTO)
        {
            if (!companyDTO.Id.HasValue)
                return Enums.Result.Error;
            var company = await _companyRepository.GetByIdAsync(companyDTO.Id);
            if(company == null)
                return Enums.Result.Error;

            company = company.Validate(companyDTO);
            var result = await _companyRepository.UpdateAsync(company);
            return result;
        }

        public async Task<Enums.Result> DeleteCompany(Guid companyId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return Enums.Result.Error;

            var result = await _companyRepository.DeleteAsync(company);
            return result;
        }
    }
}
