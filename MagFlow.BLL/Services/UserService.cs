using MagFlow.BLL.Helpers.Auth;
using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Mappers.Domain.CoreScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.Attributes;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Generators.EmailGenerators;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Auth;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Shared.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace MagFlow.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailService _emailService;
        private readonly INetworkService _networkService;
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,
            IEmailService emailService,
            ISessionRepository sessionRepository,
            ICompanyRepository companyRepository,
            INetworkService networkService,
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _companyRepository = companyRepository;
            _logger = logger;
            _userManager = userManager;
            _networkService = networkService;
            _emailService = emailService;
        }


        public async Task<UserDTO?> GetUser(Guid id, bool includeCompanies = false)
        {
            ApplicationUser? user = null;
            if (includeCompanies)
                user = await _userRepository.GetByIdAsync(id, query => query
                    .Include(x => x.Companies)
                    .ThenInclude(y => y.Company));
            else
                user = await _userRepository.GetByIdAsync(id);

            UserDTO? userDTO = user?.ToDTO();
            if (includeCompanies)
                userDTO?.Companies = user?.Companies?
                    .Where(x => x.Company != null)
                    .Select(x => x.Company!.ToDTO())
                    .ToList();
            return userDTO;
        }

        public async Task<UserDTO?> GetCurrentUser()
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return null;
            return await GetUser(userId.Value);
        }

        [MinimumRole(nameof(AppRole.SysAdmin))]
        public async Task<QueryResponse<UserDTO>> GetUsers(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _userRepository.GetAsync(new QueryOptions<ApplicationUser>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new System.Linq.Expressions.Expression<Func<ApplicationUser, string?>>[]
                {
                    u => u.FirstName, u => u.LastName, u => u.Email
                },
                SortBy = sortBy,
                Descending = descending
            }, query => query.Include(x => x.Companies).ThenInclude(y => y.Company)
                             .Include(x => x.Roles).ThenInclude(y => y.Role));
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



        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<Enums.Result> CreateUser(SignUpModel model)
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return Enums.Result.Error;
            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
            {
                _logger.LogError($"Cannot get actual user");
                return Enums.Result.Error;
            }
            if(!user.DefaultCompanyId.HasValue)
            {
                _logger.LogError($"Cannot create user because no company is selected");
                return Enums.Result.Error;
            }

            var tempUser = await _userRepository.GetByEmailAsync(model.Email);
            if (tempUser != null)
            {
                _logger.LogError($"Cannot create user because already exists ({model.Email})");
                return Enums.Result.Error;
            }

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrEmpty(model.Password) || model.Password != model.ConfirmPassword)
            {
                _logger.LogError($"Error while creating user - form is invalid");
                return Enums.Result.Error;
            }

            var now = DateTime.UtcNow;
            ApplicationUser newUser = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = model.Email.ToLower(),
                NormalizedEmail = model.Email.Normalize().ToUpper(),
                UserName = model.Email.ToLower(),
                NormalizedUserName = model.Email.Normalize().ToUpper(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = now,
                LastLogin = now,
                IsActive = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                DefaultCompanyId = user.DefaultCompanyId.Value,
                UserSettings = new ApplicationUserSettings
                {
                    Language = user.UserSettings?.Language ?? Shared.Models.Enums.Language.Polish,
                    ThemeMode = user.UserSettings?.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode,
                    DecimalSeparator = user.UserSettings?.DecimalSeparator ?? Shared.Models.Enums.DecimalSeparator.Comma,
                    DateFormat = user.UserSettings?.DateFormat ?? Shared.Models.Enums.DateFormat.DD_MM_RRRR_DOTS,
                    TimeFormat = user.UserSettings?.TimeFormat ?? Shared.Models.Enums.TimeFormat.HH_MM_24H,
                    TimeZone = user.UserSettings?.TimeZone ?? Shared.Models.Enums.TimeZone.Europe_Warsaw
                }
            };
            var password = new PasswordHasher<ApplicationUser>();
            newUser.PasswordHash = password.HashPassword(newUser, model.Password);

            var result = await _userRepository.AddAsync(newUser);
            return result;
        }

        [MinimumRole(nameof(AppRole.CompanyAdmin))]
        public async Task<Enums.Result> CreateUser(SignSomeoneUpModel model)
        {
            var password = PasswordGenerator.Generate();
            return await CreateUser(new SignUpModel()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = password,
                ConfirmPassword = password
            });
        }



        // Settings section
        public async Task<Enums.Result> UpdateUserSettings(UserSettingsDTO userSettingsDTO)
        {
            try
            {
                var userId = _networkService.GetUserId() ?? Guid.Empty;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return Enums.Result.Error;

                if (user.UserSettings == null)
                    user.UserSettings = userSettingsDTO.ToEntity(userId);
                else
                    user.UserSettings = userSettingsDTO.ToEntity(user.UserSettings);

                return await _userRepository.UpdateSettingsAsync(user.UserSettings);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while updating user settings");
                return Enums.Result.Error;
            }
        }



        // Session section
        public async Task<Enums.Result> UpdateLastSession(UserSessionDTO sessionDTO)
        {
            try
            {
                var userId = _networkService.GetUserId() ?? Guid.Empty;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return Enums.Result.Error;

                var session = await _sessionRepository.GetByIdAsync(sessionDTO.Id, s => s.Include(x => x.SessionModules).ThenInclude(y => y.Module));
                if(session == null)
                    return Enums.Result.Error;
                var dbSessionModulesIds = session.SessionModules.Select(x => x.ModuleId).ToList();
                var currentSessionModulesIds = sessionDTO.Modules.Select(x => x.Id).ToList();

                var modulesToRemove = dbSessionModulesIds.Except(currentSessionModulesIds);
                var modulesToAdd = currentSessionModulesIds.Except(dbSessionModulesIds);

                if (modulesToRemove.Any())
                {
                    List<SessionModule> sessionModules = new List<SessionModule>();
                    foreach (var moduleId in modulesToRemove)
                    {
                        var sessionModule = session.SessionModules.FirstOrDefault(x => x.ModuleId == moduleId);
                        if (sessionModule != null)
                        {
                            session.SessionModules.Remove(sessionModule);
                            sessionModules.Add(sessionModule);
                        }
                    }
                    await _sessionRepository.RemoveSessionModulesAsync(sessionModules);
                }

                if(modulesToAdd.Any())
                {
                    var userCompanyId = user.DefaultCompanyId ?? Guid.Empty;
                    var companyModules = await _companyRepository.GetCompanyModules(userCompanyId);
                    var selectedModules = companyModules?
                        .Where(x => x.Module != null && modulesToAdd.Contains(x.Module.Id))
                        .ToList() ?? new List<CompanyModule>();
                    List<SessionModule> sessionModules = new List<SessionModule>();
                    foreach (var module in selectedModules)
                    {
                        var sessionModule = new SessionModule()
                        {
                            ModuleId = module.ModuleId,
                            SessionId = session.Id
                        };
                        session.SessionModules.Add(sessionModule);
                        sessionModules.Add(sessionModule);
                    }
                    await _sessionRepository.AddSessionModulesAsync(sessionModules);
                }
                

                session.LastTimeRecord = DateTime.UtcNow;
                return await _sessionRepository.UpdateAsync(session);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while updating user session");
                return Enums.Result.Error;
            }
        }

        public async Task<Enums.Result> StartNewSession(List<ModuleDTO> modules)
        {
            try
            {
                var userId = _networkService.GetUserId() ?? Guid.Empty;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return Enums.Result.Error;
                
                var selectedModulesIds = modules.Select(m => m.Id).Distinct().ToList();
                var userCompanyId = user.DefaultCompanyId ?? Guid.Empty;
                var companyModules = await _companyRepository.GetCompanyModules(userCompanyId);
                var selectedModules = companyModules?
                    .Where(x => x.Module != null && selectedModulesIds.Contains(x.Module.Id))
                    .ToList() ?? new List<CompanyModule>();
                if(!selectedModules.Any())
                    return Enums.Result.Error;

                List<SessionModule> sessionModules = new List<SessionModule>();
                foreach (var module in selectedModules)
                {
                    sessionModules.Add(new SessionModule()
                    {
                        ModuleId = module.ModuleId
                    });
                }
                var now = DateTime.UtcNow;
                UserSession newSession = new UserSession()
                {
                    UserId = userId,
                    CreatedDate = now,
                    LastTimeRecord = now,
                    ExpiresAt = DateTime.MaxValue,
                    SessionModules = sessionModules,
                    IpAddress = _networkService.GetUserIp() ?? "Unknown",
                    UserAgent = "Website",
                    RefreshToken = Guid.NewGuid().ToString()
                };
                return await _sessionRepository.AddAsync(newSession);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while starting user session");
                return Enums.Result.Error;
            }
        }
        
        public async Task<List<ModuleDTO>?> GetLastSessionModules()
        {
            try
            {
                var userId = _networkService.GetUserId() ?? Guid.Empty;
                var session = (await _userRepository.GetLastSessionsAsync(userId))?.FirstOrDefault();
                if (session == null)
                    return null;
                var sessionModules = await _sessionRepository.GetSessionModules(session.Id);
                if(sessionModules == null)
                    return null;
                return sessionModules.ToDTO();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while getting user session");
                return null;
            }
        }

        public async Task<List<UserSessionDTO>?> GetLastSessions(int historyLength = 1)
        {
            try
            {
                var userId = _networkService.GetUserId() ?? Guid.Empty;
                var userSessions = await _userRepository.GetLastSessionsAsync(userId, historyLength);
                return userSessions?.ToDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting user session");
                return null;
            }
        }



        // Password section
        public async Task ResetPasswordRequest(ForgotPasswordModel model)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Password reset requested for non-existing email: {model.Email}");
                    return;
                }
                if (AppSettings.AppUri == null || string.IsNullOrEmpty(AppSettings.AppUri.AbsoluteUri))
                {
                    _logger.LogError("Error while generating password reset link: AppUri is not configured.");
                    return;
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = HttpUtility.UrlEncode(token);
                var callback = new Uri(AppSettings.AppUri, "/ForgotPasswordChange?userId=" + user.Id + "&token=" + encodedToken);
                await _emailService.SendPasswordResetLinkAsync(user, model.Email, callback.ToString());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while processing reset password request");
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.Password))
                    return false;

                var userId = _networkService.GetUserId() ?? Guid.Empty;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                foreach(var validator in _userManager.PasswordValidators)
                {
                    var validationResult = await validator.ValidateAsync(_userManager, user, model.Password);
                    if (!validationResult.Succeeded)
                    {
                        _logger.LogWarning("Password validation failed for user {UserId}: {Errors}", user.Id, string.Join(", ", validationResult.Errors.Select(e => e.Description)));
                        return false;
                    }
                }

                var changeResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                if(changeResult.Succeeded)
                {
                    try
                    {
                        await _emailService.SendAsync(user.UserName, user.Email, "Password Changed", EmailGenerator.PasswordChangedBody(user.FirstName, user.LastName, user.Email, user.UserSettings?.Language));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to send password change notification email for user {UserId}", user.Id);
                    }
                    return true;
                }

                _logger.LogWarning("Password change failed for user {UserId}: {Errors}", user.Id, string.Join(", ", changeResult.Errors.Select(e => e.Description)));
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while processing change password request");
                return false;
            }
        }

        public async Task<bool> ChangePassword(TokenChangePasswordModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.Password))
                    return false;

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return false;

                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var isValid = await passwordValidator.ValidateAsync(_userManager, user, model.Password);
                if (!isValid.Succeeded)
                    return false;

                var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resetResult.Succeeded)
                {
                    try
                    {
                        await _emailService.SendAsync(user.UserName, user.Email, "Password Changed", EmailGenerator.PasswordChangedBody(user.FirstName, user.LastName, user.Email, user.UserSettings?.Language));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to send password change notification email for user {UserId}", user.Id);
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while changing user password");
            }
            return false;
        }
    }
}
