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
            return new UserDTO()
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Settings = ToDTO(applicationUser.UserSettings)
            };
        }

        public static UserSettingsDTO ToDTO(this ApplicationUserSettings? applicationUserSettings)
        {
            return new UserSettingsDTO()
            {
                Language = applicationUserSettings?.Language ?? Shared.Models.Enums.Language.Polish,
            };
        }

        public static UserSessionDTO ToDTO(this UserSession userSession)
        {
            return new UserSessionDTO()
            {
                Id = userSession.Id,
                ExpiresAt = userSession.ExpiresAt,
                CreatedAt = userSession.CreatedDate,
                Modules = userSession.SessionModules?.Where(x => x.Module != null)?.Select(m => m.Module!).ToDTO() ?? new List<ModuleDTO>()
            };
        }
    }
}
