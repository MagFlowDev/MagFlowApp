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

namespace MagFlow.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,
            IEmailService emailService,
            ISessionRepository sessionRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _logger = logger;
            _userManager = userManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDTO?> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDTO();
        }

        public async Task<List<ModuleDTO>?> GetLastSessionModules()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToGuid();
                var session = await _userRepository.GetLastSessionAsync(userId ?? Guid.Empty);
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

        public async Task<UserSessionDTO?> GetLastSession()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToGuid();
                var userSession = await _userRepository.GetLastSessionAsync(userId ?? Guid.Empty);
                return userSession?.ToDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting user session");
                return null;
            }
        }



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
