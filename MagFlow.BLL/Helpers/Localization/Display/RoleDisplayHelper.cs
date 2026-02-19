using MagFlow.Shared.Models.Enumerators;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Helpers.Localization.Display
{
    public static class RoleDisplayHelper
    {
        public static string DisplayRole(this AppRole role, IStringLocalizer localizer)
        {
            string? roleName = null;

            if (role == AppRole.Foreman) roleName = "RoleForeman";
            else if (role == AppRole.Operator) roleName = "RoleOperator";
            else if (role == AppRole.Supervisor) roleName = "RoleSupervisor";
            else if (role == AppRole.Auditor) roleName = "RoleAuditor";
            else if (role == AppRole.CompanyAdmin) roleName = "RoleCompanyAdmin";
            else if (role == AppRole.SysAdmin) roleName = "RoleSysAdmin";
            else if (role == AppRole.SuperAdmin) roleName = "RoleSuperAdmin";

            if (string.IsNullOrWhiteSpace(roleName))
                return string.Empty;

            return localizer[roleName];
        }

        public static string DisplayUser(this AppRole role, IStringLocalizer localizer)
        {
            string? roleName = null;

            if (role == AppRole.Foreman) roleName = "UserForeman";
            else if (role == AppRole.Operator) roleName = "UserOperator";
            else if (role == AppRole.Supervisor) roleName = "UserSupervisor";
            else if (role == AppRole.Auditor) roleName = "UserAuditor";
            else if (role == AppRole.CompanyAdmin) roleName = "UserCompanyAdmin";
            else if (role == AppRole.SysAdmin) roleName = "UserSysAdmin";
            else if (role == AppRole.SuperAdmin) roleName = "UserSuperAdmin";

            if (string.IsNullOrWhiteSpace(roleName))
                return string.Empty;

            return localizer[roleName];
        }
    }
}
