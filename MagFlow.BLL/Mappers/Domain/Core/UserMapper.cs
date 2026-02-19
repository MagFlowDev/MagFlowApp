using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Domain.Core
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this ApplicationUser applicationUser)
        {
            var roles = applicationUser.Roles?.Select(x => RoleMapper.GetAppRole(x.RoleId));
            return new UserDTO()
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Email = applicationUser.Email ?? "",
                CreatedAt = applicationUser.CreatedAt,
                IsActive = applicationUser.IsActive,
                Roles = roles?.Where(x => x != null).Select(x => x!).ToList() ?? new List<Shared.Models.Enumerators.AppRole>(),
                Settings = ToDTO(applicationUser.UserSettings)
            };
        }

        public static UserSettingsDTO ToDTO(this ApplicationUserSettings? applicationUserSettings)
        {
            return new UserSettingsDTO()
            {
                Language = applicationUserSettings?.Language ?? Shared.Models.Enums.Language.Polish,
                ThemeMode = applicationUserSettings?.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode
            };
        }

        public static ApplicationUserSettings ToEntity(this UserSettingsDTO userSettingsDTO, Guid userId)
        {
            return new ApplicationUserSettings()
            {
                Language = userSettingsDTO.Language ?? Shared.Models.Enums.Language.Polish,
                ThemeMode = userSettingsDTO.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode,
                UserId = userId,
            };
        }

        public static ApplicationUserSettings ToEntity(this UserSettingsDTO userSettingsDTO, ApplicationUserSettings actualSettings)
        {
            actualSettings.Language = userSettingsDTO.Language ?? actualSettings.Language;
            actualSettings.ThemeMode = userSettingsDTO.ThemeMode ?? actualSettings.ThemeMode;
            return actualSettings;
        }




        public static UserSessionDTO ToDTO(this UserSession userSession)
        {
            return new UserSessionDTO()
            {
                Id = userSession.Id,
                ExpiresAt = userSession.ExpiresAt,
                CreatedAt = userSession.CreatedDate,
                LastTimeRecord = userSession.LastTimeRecord,
                Modules = userSession.SessionModules?.Where(x => x.Module != null)?.Select(m => m.Module!).ToDTO() ?? new List<ModuleDTO>()
            };
        }

        public static List<UserSessionDTO> ToDTO(this IEnumerable<UserSession> userSessions)
        {
            return userSessions.Select(x => x.ToDTO()).ToList();
        }
    }
}
