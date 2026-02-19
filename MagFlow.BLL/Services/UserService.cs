using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Mappers.Domain.Core;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.Shared.Attributes;
using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Generators.EmailGenerators;
using MagFlow.Shared.Models.Auth;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Shared.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Web;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;

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
        }


        public async Task<UserDTO?> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDTO();
        }

        public async Task<UserDTO?> GetCurrentUser()
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return null;
            return await GetUser(userId.Value);
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
            return false;
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
                    await _emailService.SendAsync(user.UserName, user.Email, "Password Changed", EmailGenerator.PasswordChangedBody(user.FirstName, user.LastName, user.Email, user.UserSettings?.Language));
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
