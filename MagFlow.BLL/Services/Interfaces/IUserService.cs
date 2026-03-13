using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
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
        Task<QueryResponse<UserDTO>> GetUsers(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);

        Task<Enums.Result> CreateUser(SignUpModel model);

        Task<Enums.Result> UpdateUserSettings(UserSettingsDTO userSettingsDTO);

        Task ResetPasswordRequest(ForgotPasswordModel model);
        Task<bool> ChangePassword(ChangePasswordModel model);
        Task<bool> ChangePassword(TokenChangePasswordModel model);

        Task<List<UserSessionDTO>?> GetLastSessions(int historyLength = 1);
        Task<Enums.Result> UpdateLastSession(UserSessionDTO sessionDTO);
        Task<Enums.Result> StartNewSession(List<ModuleDTO> modules);
    }
}
