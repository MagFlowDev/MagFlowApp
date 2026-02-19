using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.Models;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUser(Guid id);
        Task<UserDTO?> GetCurrentUser();

        Task<Enums.Result> UpdateUserSettings(UserSettingsDTO userSettingsDTO);

        Task ResetPasswordRequest(ForgotPasswordModel model);
        Task<bool> ChangePassword(ChangePasswordModel model);
        Task<bool> ChangePassword(TokenChangePasswordModel model);

        Task<List<UserSessionDTO>?> GetLastSessions(int historyLength = 1);
        Task<Enums.Result> UpdateLastSession(UserSessionDTO sessionDTO);
        Task<Enums.Result> StartNewSession(List<ModuleDTO> modules);
    }
}
