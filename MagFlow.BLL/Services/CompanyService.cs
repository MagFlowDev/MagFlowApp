using MagFlow.BLL.Helpers.Auth;
using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Mappers.Domain.CoreScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.BLL.Services.Notifications;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.Attributes;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Generators.EmailGenerators;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static MagFlow.Shared.Models.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagFlow.BLL.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IWorkDayRepository _workDayRepository;
        private readonly IWorkingHourRepository _workingHourRepository;
        private readonly IUserRepository _userRepository;
        private readonly INetworkService _networkService;
        private readonly IModuleRepository _moduleRepository;
        private readonly IEmailService _emailService;
        private readonly IServerNotificationService _serverNotificationService;
        private readonly IUserRevocationService _userRevocationService;

        private readonly ILogger<CompanyService> _logger;

        public CompanyService(ICompanyRepository companyRepository,
            INetworkService networkService,
            IUserRepository userRepository,
            IWorkDayRepository workDayRepository,
            IWorkingHourRepository workingHourRepository,
            IModuleRepository moduleRepository,
            IEmailService emailService,
            IServerNotificationService serverNotificationService,
            IUserRevocationService userRevocationService,
            ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _networkService = networkService;
            _workDayRepository = workDayRepository;
            _workingHourRepository = workingHourRepository;
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
            _emailService = emailService;
            _serverNotificationService = serverNotificationService;
            _userRevocationService = userRevocationService;
            _logger = logger;
        }

        public async Task<CompanyDTO?> GetCurrentCompany()
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return null;
                var user = await _userRepository.GetByIdAsync(userId.Value);

                var companyId = user?.DefaultCompanyId;
                if (!companyId.HasValue)
                    return null;

                var company = await _companyRepository.GetByIdAsync(companyId.Value, s => s
                    .Include(x => x.CompanySettings)
                    .Include(x => x.Logo)
                    .Include(x => x.Modules).ThenInclude(y => y.Module));

                return company?.ToDTO();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to get user company");
                return null;
            }
        }

        [MinimumRole(nameof(AppRole.SysAdmin))]
        public async Task<CompanyDTO?> GetCompany(Guid companyId)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(companyId, s => s
                    .Include(x => x.CompanySettings)
                    .Include(x => x.Logo)
                    .Include(x => x.Modules).ThenInclude(y => y.Module));

                return company?.ToDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user company");
                return null;
            }
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

        public async Task<List<ModuleDTO>?> GetAllModules()
        {
            var modules = await _moduleRepository.GetAllAsync();
            return modules?.ToDTO();
        }
        
        public async Task<List<DefaultWorkingHourDTO>?> GetDefaultWorkingHours()
        {
            var result = await _workingHourRepository.GetAllAsync();
            return result?.ToDTO();
        }

        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<QueryResponse<UserDTO>> GetUsers(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _userRepository.GetCompanyUsersAsync(new QueryOptions<User>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new Expression<Func<User, string?>>[]
                {
                    u => u.FirstName, u => u.LastName, u => u.Email
                },
                SortBy = sortBy,
                Descending = descending
            });
            return new QueryResponse<UserDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    dto.Companies = x.Companies
                        .Where(y => y.Company != null)
                        .Select(y => y.Company!.ToDTO())
                        .ToList();
                    return dto;
                }).ToList() ?? new List<UserDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        [MinimumRole(nameof(AppRole.SysAdmin))]
        public async Task<QueryResponse<CompanyDTO>> GetCompanies(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _companyRepository.GetAsync(new QueryOptions<Company>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new System.Linq.Expressions.Expression<Func<Company, string?>>[]
                {
                    s => s.Name, s => s.Address.Line1, s => s.Address.ZipCode, s => s.Address.City, s => s.Address.Country, s => s.TaxNumber
                },
                SortBy = sortBy,
                Descending = descending
            }, query => query.Include(x => x.Modules));
            return new QueryResponse<CompanyDTO>()
            {
                Elements = queryResponse?.Elements
                    .Select(x => x.ToDTO())
                    .ToList() ?? new List<CompanyDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }



        [MinimumRole(nameof(AppRole.SysAdmin))]
        public async Task<Enums.Result> CreateCompany(CompanyDTO companyDTO)
        {
            Company company = companyDTO.ToEntity();
            if (string.IsNullOrEmpty(company.Name) || string.IsNullOrEmpty(company.ConnectionString))
                return Enums.Result.Error;

            var result = await _companyRepository.AddAsync(company);
            return result;
        }



        // Settings section
        [MinimumRole(nameof(AppRole.CompanyAdmin))]
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

        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<Enums.Result> UpdateCompanySettings(Guid companyId, CompanySettingsDTO companySettingsDTO)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return Enums.Result.Error;

            if (company.CompanySettings == null)
                company.CompanySettings = companySettingsDTO.ToEntity(companyId);
            else
                company.CompanySettings = companySettingsDTO.ToEntity(company.CompanySettings);

            return await _companyRepository.UpdateSettingsAsync(company.CompanySettings);
        }

        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<Enums.Result> UpdateCompanyLogo(Guid companyId, byte[] data, string contentType = "image/jpg")
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return Enums.Result.Error;

            var result = await _companyRepository.UpdateLogoAsync(companyId, data, contentType);
            return result;
        }

        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<Enums.Result> RemoveCompanyLogo(Guid companyId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return Enums.Result.Error;

            var result = await _companyRepository.RemoveLogoAsync(companyId);
            return result;
        }

        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<Enums.Result> UpdateDefaultWorkingHours(List<DefaultWorkingHourDTO> defaultWorkingHourDTOs)
        {
            var existinDefaultWorkingHours = await _workingHourRepository.GetAllAsync();
            var updatedDefaultWorkingHours = defaultWorkingHourDTOs.ToEntity(existinDefaultWorkingHours);
            foreach(var workingHours in updatedDefaultWorkingHours)
            {
                if(workingHours.IsClosed)
                {
                    workingHours.OpenTime = null;
                    workingHours.CloseTime = null;
                }
            }

            var result = await _workingHourRepository.UpdateRangeAsync(updatedDefaultWorkingHours);
            return result;
        }

        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<Enums.Result> UpdateWorkDays(List<WorkDayDTO> workDayDTOs)
        {
            var existingWorkDays = await _workDayRepository.GetAllAsync();
            var updatedWorkDays = workDayDTOs.ToEntity(existingWorkDays);

            var result = await _workDayRepository.UpdateRangeAsync(existingWorkDays);
            return result;
        }



        [MinimumRole(nameof(AppRole.SysAdmin))]
        public async Task<Enums.Result> DeleteCompany(Guid companyId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return Enums.Result.Error;

            var companyUsers = await _companyRepository.RemoveAllUsersFromCompanyAsync(company);
            if (companyUsers == null)
                return Enums.Result.Error;

            try
            {
                List<string> userIds = companyUsers!.Select(x => x.ToString()).ToList();
                await _serverNotificationService.ForceUserLogoutAsync(userIds);
                userIds.ForEach(x => _userRevocationService.RevokeUser(x));
            }
            catch { }

            var result = await _companyRepository.DeleteAsync(company);
            return result;
        }



        // Requests section
        public async Task<Enums.Result> SendExtendModuleRequest(Guid companyId, Guid moduleId, string user, string message)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(companyId);
                if (company == null)
                    return Enums.Result.Error;

                await _emailService.SendToMeAsync("Extend module", EmailGenerator.SimpleCompanyMessage(user, $"{company.Name} ({companyId})", message));
                return Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message");
                return Result.Error;
            }
        }

        public async Task<Enums.Result> SendAskForModuleOfferRequest(Guid companyId, Guid moduleId, string user, string message)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(companyId);
                if (company == null)
                    return Enums.Result.Error;

                await _emailService.SendToMeAsync("Ask for offer", EmailGenerator.SimpleCompanyMessage(user, $"{company.Name} ({companyId})", message));
                return Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message");
                return Result.Error;
            }
        }
    }
}
