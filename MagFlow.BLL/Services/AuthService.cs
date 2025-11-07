using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF.MultiTenancy;
using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public AuthService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ICompanyContext companyContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _companyContext = companyContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return SignInResult.Failed;

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
            try
            {
                if (!result.Succeeded || string.IsNullOrEmpty(user.Email) || !user.DefaultCompanyId.HasValue)
                    return result;
                await _companyContext.SetCompanyContext(user.Email);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
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
            }
            catch(Exception ex)
            {
                
            }
            return result;
        }
    }
}
