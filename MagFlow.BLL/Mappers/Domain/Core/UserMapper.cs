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
                ThemeMode = applicationUserSettings?.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode,
                DecimalSeparator = applicationUserSettings?.DecimalSeparator ?? Shared.Models.Enums.DecimalSeparator.Comma,
                DateFormat = applicationUserSettings?.DateFormat ?? Shared.Models.Enums.DateFormat.DD_MM_RRRR_DOTS,
                TimeFormat = applicationUserSettings?.TimeFormat ?? Shared.Models.Enums.TimeFormat.HH_MM_24H,
                TimeZone = applicationUserSettings?.TimeZone ?? Shared.Models.Enums.TimeZone.Europe_Warsaw,
                SystemAlertsEnabled = applicationUserSettings?.SystemAlertsEnabled ?? false,
                ProductionNotificationsEnabled = applicationUserSettings?.ProductionNotificationsEnabled ?? false,
                EmailNotificationsEnabled = applicationUserSettings?.EmailNotificationsEnabled ?? false,
            };
        }

        public static ApplicationUserSettings ToEntity(this UserSettingsDTO userSettingsDTO, Guid userId)
        {
            return new ApplicationUserSettings()
            {
                Language = userSettingsDTO.Language ?? Shared.Models.Enums.Language.Polish,
                ThemeMode = userSettingsDTO.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode,
                DecimalSeparator = userSettingsDTO.DecimalSeparator ?? Shared.Models.Enums.DecimalSeparator.Comma,
                DateFormat = userSettingsDTO.DateFormat ?? Shared.Models.Enums.DateFormat.DD_MM_RRRR_DOTS,
                TimeFormat = userSettingsDTO.TimeFormat ?? Shared.Models.Enums.TimeFormat.HH_MM_24H,
                TimeZone = userSettingsDTO.TimeZone ?? Shared.Models.Enums.TimeZone.Europe_Warsaw,
                SystemAlertsEnabled = userSettingsDTO?.SystemAlertsEnabled ?? false,
                ProductionNotificationsEnabled = userSettingsDTO?.ProductionNotificationsEnabled ?? false,
                EmailNotificationsEnabled = userSettingsDTO?.EmailNotificationsEnabled ?? false,
                UserId = userId,
            };
        }

        public static ApplicationUserSettings ToEntity(this UserSettingsDTO userSettingsDTO, ApplicationUserSettings actualSettings)
        {
            actualSettings.Language = userSettingsDTO.Language ?? actualSettings.Language;
            actualSettings.ThemeMode = userSettingsDTO.ThemeMode ?? actualSettings.ThemeMode;
            actualSettings.DecimalSeparator = userSettingsDTO.DecimalSeparator ?? actualSettings.DecimalSeparator;
            actualSettings.DateFormat = userSettingsDTO.DateFormat ?? actualSettings.DateFormat;
            actualSettings.TimeFormat = userSettingsDTO.TimeFormat ?? actualSettings.TimeFormat;
            actualSettings.TimeZone = userSettingsDTO.TimeZone ?? actualSettings.TimeZone;
            actualSettings.SystemAlertsEnabled = userSettingsDTO.SystemAlertsEnabled ?? actualSettings.SystemAlertsEnabled;
            actualSettings.ProductionNotificationsEnabled = userSettingsDTO.ProductionNotificationsEnabled ?? actualSettings.ProductionNotificationsEnabled;
            actualSettings.EmailNotificationsEnabled = userSettingsDTO.EmailNotificationsEnabled ?? actualSettings.EmailNotificationsEnabled;
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
