using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF.MultiTenancy;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyContext _companyContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        private readonly IEventService _eventService;

        public AuthService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ICompanyContext companyContext,
            IHttpContextAccessor httpContextAccessor,
            IEventService eventService,
            ILogger<AuthService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _companyContext = companyContext;
            _httpContextAccessor = httpContextAccessor;
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe, string? ip = null, string? agent = null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                await _eventService.AddEventAsync(Guid.Empty, Enums.EventLogCategory.Logging, Enums.EventLogLevel.WARN, "Logging attempt failed", $"User tried to login with user email: {email}", ip ?? string.Empty, agent ?? string.Empty);
                return SignInResult.Failed;
            }
            

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
            try
            {
                if (!result.Succeeded || string.IsNullOrEmpty(user.Email) || !user.DefaultCompanyId.HasValue)
                {
                    await _eventService.AddEventAsync(Guid.Empty, Enums.EventLogCategory.Logging, Enums.EventLogLevel.WARN, "Logging attempt failed", $"User tried to login with user email: {email}", ip ?? string.Empty, agent ?? string.Empty);
                    return result;
                }
                await _companyContext.SetCompanyContext(user.Email);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(Claims.CompanyClaim, user.DefaultCompanyId.Value.ToString())
                };

                var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity);

                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
                {
                    await _httpContextAccessor.HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                        principal,
                        new AuthenticationProperties { IsPersistent = rememberMe }
                    );
                }

                await _eventService.AddEventAsync(Guid.Empty, Enums.EventLogCategory.Logging, Enums.EventLogLevel.INFO, "Logging attempt successfull", $"User {email} logged in", ip ?? string.Empty, agent ?? string.Empty);
            }
            catch(Exception ex)
            {
                
            }
            return result;
        }
    }
}
