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
using System.ComponentModel.Design;
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
        public async Task<Enums.Result> UpdateModulesLicense(CompanyDTO companyDTO, List<Guid> moduleIds, DateTime expireDate, bool activate = false)
        {
            if (!companyDTO.Id.HasValue)
                return Enums.Result.Error;
            if (expireDate < DateTime.UtcNow.Date.AddDays(1))
                return Enums.Result.Error;
            var company = await _companyRepository.GetByIdAsync(companyDTO.Id.Value, s => s
                   .Include(x => x.Modules).ThenInclude(y => y.Module));
            if (company == null)
                return Enums.Result.Error;

            var modules = await _moduleRepository.GetAllAsync();
            var existingModules = company.Modules.Where(x => moduleIds.Contains(x.ModuleId)).ToList();
            var modulesToActivateIds = moduleIds.Except(existingModules.Select(x => x.ModuleId));
            var modulesToActivate = modules.Where(x => modulesToActivateIds.Contains(x.Id)).ToList();

            List<CompanyModule> modulesToAdd = new List<CompanyModule>();
            List<CompanyModule> modulesToUpdate = new List<CompanyModule>();

            var now = DateTime.UtcNow;
            expireDate = new DateTime(expireDate.Year, expireDate.Month, expireDate.Day, 23, 59, 59);
            foreach (var module in modulesToActivate)
            {
                CompanyModule companyModule = new CompanyModule()
                {
                    Id = Guid.NewGuid(),
                    CompanyId = company.Id,
                    AssignedAt = now,
                    IsActive = true,
                    ModuleId = module.Id,
                    EnabledFrom = now,
                    EnabledTo = expireDate,
                };
                modulesToAdd.Add(companyModule);
            }
            existingModules.ForEach(x =>
            {
                x.EnabledTo = expireDate;
                x.IsActive = true;
                modulesToUpdate.Add(x);
            });

            var result = await _companyRepository.UpdateCompanyModules(modulesToUpdate);
            if(result != Enums.Result.Success)
                return result;
            result = await _companyRepository.AddCompanyModules(modulesToAdd);
            return result;
        }

        [MinimumRole(nameof(AppRole.SysAdmin))]
        public async Task<Enums.Result> UpdateModulesLicense(CompanyDTO companyDTO, List<Guid> moduleIds, Enums.LongTimePeriod timePeriod, bool activate = false)
        {
            if (!companyDTO.Id.HasValue)
                return Enums.Result.Error;
            var company = await _companyRepository.GetByIdAsync(companyDTO.Id.Value, s => s
                   .Include(x => x.Modules).ThenInclude(y => y.Module));
            if (company == null)
                return Enums.Result.Error;

            var modules = await _moduleRepository.GetAllAsync();
            var existingModules = company.Modules.Where(x => moduleIds.Contains(x.ModuleId)).ToList();
            var modulesToActivateIds = moduleIds.Except(existingModules.Select(x => x.ModuleId));
            var modulesToActivate = modules.Where(x => modulesToActivateIds.Contains(x.Id)).ToList();

            List<CompanyModule> modulesToAdd = new List<CompanyModule>();
            List<CompanyModule> modulesToUpdate = new List<CompanyModule>();

            var now = DateTime.UtcNow;
            var expireDate = timePeriod switch
            {
                Enums.LongTimePeriod.Month => now.AddMonths(1),
                Enums.LongTimePeriod.Quarter => now.AddMonths(3),
                Enums.LongTimePeriod.Year => now.AddYears(1),
                _ => now
            };
            expireDate = new DateTime(expireDate.Year, expireDate.Month, expireDate.Day, 23, 59, 59);
            foreach (var module in modulesToActivate)
            {
                CompanyModule companyModule = new CompanyModule()
                {
                    Id = Guid.NewGuid(),
                    CompanyId = company.Id,
                    AssignedAt = now,
                    IsActive = true,
                    ModuleId = module.Id,
                    EnabledFrom = now,
                    EnabledTo = expireDate,
                };
                modulesToAdd.Add(companyModule);
            }
            existingModules.ForEach(x =>
            {
                if (x.EnabledTo > now.AddYears(100)) x.EnabledTo = x.EnabledTo;
                else if (x.EnabledTo < now) x.EnabledTo = expireDate;
                else if(timePeriod == LongTimePeriod.Month) x.EnabledTo = x.EnabledTo.AddMonths(1);
                else if(timePeriod == LongTimePeriod.Quarter) x.EnabledTo = x.EnabledTo.AddMonths(3);
                else if(timePeriod == LongTimePeriod.Year) x.EnabledTo = x.EnabledTo.AddYears(1);
                x.IsActive = true;
                modulesToUpdate.Add(x);
            });

            var result = await _companyRepository.UpdateCompanyModules(modulesToUpdate);
            if (result != Enums.Result.Success)
                return result;
            result = await _companyRepository.AddCompanyModules(modulesToAdd);
            return result;
        }



        [MinimumRole(nameof(AppRole.SysAdmin))]
        public async Task<Enums.Result> DeleteCompany(CompanyDTO companyDTO)
        {
            if (companyDTO == null || !companyDTO.Id.HasValue)
                return Enums.Result.Error;
            var company = await _companyRepository.GetByIdAsync(companyDTO.Id.Value);
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
