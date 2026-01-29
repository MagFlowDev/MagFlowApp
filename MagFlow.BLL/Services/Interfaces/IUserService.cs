using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUser(Guid id);

        Task ResetPasswordRequest(ForgotPasswordModel model);
        Task<bool> ChangePassword(ChangePasswordModel model);
        Task<bool> ChangePassword(TokenChangePasswordModel model);

        Task<UserSessionDTO?> GetLastSession();
    }
}
