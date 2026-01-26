using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Mappers.Domain.Core;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Models.Auth;
using MagFlow.Shared.Models.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MagFlow.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUserRepository userRepository,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<UserDTO?> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDTO();
        }

        public async Task ResetPasswordRequest(ForgotPasswordModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if(user == null)
            {
                _logger.LogWarning($"Password reset requested for non-existing email: {model.Email}");
                return;
            }
            if(AppSettings.AppUri == null || string.IsNullOrEmpty(AppSettings.AppUri.AbsoluteUri))
            {
                _logger.LogError("Error while generating password reset link: AppUri is not configured.");
                return;
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var callback = new Uri(AppSettings.AppUri, "/ForgotPasswordChange?userId=" + user.Id + "&token=" + encodedToken);
            await _emailService.SendPasswordResetLinkAsync(user, model.Email, callback.ToString());
        }

        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            return false;
        }

        public async Task<bool> ChangePassword(TokenChangePasswordModel model)
        {
            return false;
        }
    }
}
