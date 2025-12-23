using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe, string? ip = null, string? agent = null);
    }
}
