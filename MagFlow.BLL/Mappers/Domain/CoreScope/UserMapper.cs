using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Domain.CoreScope
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this ApplicationUser applicationUser)
        {
            var roles = applicationUser.Roles?.Select(x => RoleMapper.GetAppRole(x.RoleId));
            return new UserDTO()
            {
                Id = applicationUser.Id,
                CurrentCompanyId = applicationUser.DefaultCompanyId,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Email = applicationUser.Email ?? "",
                PhoneNumber = applicationUser.PhoneNumber,
                CreatedAt = applicationUser.CreatedAt,
                LastLogin = applicationUser.LastLogin,
                IsActive = applicationUser.IsActive,
                Roles = roles?.Where(x => x != null).Select(x => x!).ToList() ?? new List<Shared.Models.Enumerators.AppRole>(),
                Settings = ToDTO(applicationUser.UserSettings)
            };
        }

        public static ApplicationUser ToEntity(this UserDTO userDTO)
        {
            var now = DateTime.UtcNow;
            var id = userDTO.Id != Guid.Empty ? userDTO.Id : Guid.NewGuid();
            return new ApplicationUser()
            {
                Id = id,
                CreatedAt = userDTO.CreatedAt ?? now,
                LastLogin = userDTO.LastLogin ?? now,
                IsActive = userDTO.IsActive,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                UserSettings = userDTO.Settings.ToEntity(id),
            };
        }

        public static ApplicationUser ToEntity(this CompanyFormModel model, Guid companyId, ApplicationRole? role = null, UserDTO? actualUser = null)
        {
            var uid = Guid.NewGuid();
            List<ApplicationUserRole> roles = new List<ApplicationUserRole>();
            if (role != null)
                roles.Add(new ApplicationUserRole()
                {
                    RoleId = role.Id,
                    UserId = uid
                });
            return new ApplicationUser()
            {
                Id = uid,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                FirstName = model.AdminAccount.FirstName,
                LastName = model.AdminAccount.LastName,
                UserName = model.AdminAccount.Email,
                NormalizedUserName = model.AdminAccount.Email.Normalize().ToUpper(),
                Email = model.AdminAccount.Email,
                NormalizedEmail = model.AdminAccount.Email.Normalize().ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                EmailConfirmed = true,
                DefaultCompanyId = companyId,
                Companies = new List<CompanyUser>()
                {
                    new CompanyUser(){ AssignedAt = DateTime.UtcNow, UserId = uid, CompanyId = companyId }
                },
                Roles = roles,
                UserSettings = new ApplicationUserSettings()
                {
                    Language = actualUser?.Settings?.Language ?? Shared.Models.Enums.Language.Polish,
                    ThemeMode = actualUser?.Settings?.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode,
                    DecimalSeparator = actualUser?.Settings?.DecimalSeparator ?? Shared.Models.Enums.DecimalSeparator.Comma,
                    DateFormat = actualUser?.Settings?.DateFormat ?? Shared.Models.Enums.DateFormat.DD_MM_RRRR_DOTS,
                    TimeFormat = actualUser?.Settings?.TimeFormat ?? Shared.Models.Enums.TimeFormat.HH_MM_24H,
                    TimeZone = actualUser?.Settings?.TimeZone ?? Shared.Models.Enums.TimeZone.Europe_Warsaw,
                    SystemAlertsEnabled = actualUser?.Settings?.SystemAlertsEnabled ?? false,
                    ProductionNotificationsEnabled = actualUser?.Settings?.ProductionNotificationsEnabled ?? false,
                    EmailNotificationsEnabled = actualUser?.Settings?.EmailNotificationsEnabled ?? false,
                },
            };
        }

        public static ApplicationUser ToEntity(this UserFormModel model, Guid companyId, UserDTO? actualUser = null, ApplicationRole? role = null)
        {
            var uid = Guid.NewGuid();
            List<ApplicationUserRole> roles = new List<ApplicationUserRole>();
            if (role != null)
                roles.Add(new ApplicationUserRole()
                {
                    RoleId = role.Id,
                    UserId = uid
                });
            return new ApplicationUser()
            {
                Id = uid,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                FirstName = model.GeneralInformation.FirstName,
                PhoneNumber = model.GeneralInformation.PhoneNumber,
                LastName = model.GeneralInformation.LastName,
                UserName = model.GeneralInformation.Email,
                NormalizedUserName = model.GeneralInformation.Email.Normalize().ToUpper(),
                Email = model.GeneralInformation.Email,
                NormalizedEmail = model.GeneralInformation.Email.Normalize().ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                DefaultCompanyId = companyId,
                Companies = new List<CompanyUser>()
                {
                    new CompanyUser(){ AssignedAt = DateTime.UtcNow, UserId = uid, CompanyId = companyId }
                },
                Roles = roles,
                UserSettings = new ApplicationUserSettings()
                {
                    Language = actualUser?.Settings?.Language ?? Shared.Models.Enums.Language.Polish,
                    ThemeMode = actualUser?.Settings?.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode,
                    DecimalSeparator = actualUser?.Settings?.DecimalSeparator ?? Shared.Models.Enums.DecimalSeparator.Comma,
                    DateFormat = actualUser?.Settings?.DateFormat ?? Shared.Models.Enums.DateFormat.DD_MM_RRRR_DOTS,
                    TimeFormat = actualUser?.Settings?.TimeFormat ?? Shared.Models.Enums.TimeFormat.HH_MM_24H,
                    TimeZone = actualUser?.Settings?.TimeZone ?? Shared.Models.Enums.TimeZone.Europe_Warsaw,
                    SystemAlertsEnabled = actualUser?.Settings?.SystemAlertsEnabled ?? false,
                    ProductionNotificationsEnabled = actualUser?.Settings?.ProductionNotificationsEnabled ?? false,
                    EmailNotificationsEnabled = actualUser?.Settings?.EmailNotificationsEnabled ?? false,
                },
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
