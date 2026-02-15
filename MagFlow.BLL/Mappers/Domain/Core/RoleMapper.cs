using MagFlow.Shared.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Domain.Core
{
    public static class RoleMapper
    {
        public static AppRole? GetAppRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return null;
            switch(roleName)
            {
                case "Foreman": return AppRole.Foreman;
                case "Operator": return AppRole.Operator;
                case "Supervisor": return AppRole.Supervisor;
                case "Auditor": return AppRole.Auditor;
                case "CompanyAdmin": return AppRole.CompanyAdmin;
                case "SysAdmin": return AppRole.SysAdmin;
                case "SuperAdmin": return AppRole.SuperAdmin;

                default: return null;
            }
        }

        public static AppRole? GetAppRole(Guid roleId)
        {
            if (roleId == Shared.Constants.Identificators.RoleID.Foreman) return AppRole.Foreman;
            else if (roleId == Shared.Constants.Identificators.RoleID.Operator) return AppRole.Operator;
            else if (roleId == Shared.Constants.Identificators.RoleID.Supervisor) return AppRole.Supervisor;
            else if (roleId == Shared.Constants.Identificators.RoleID.Auditor) return AppRole.Auditor;
            else if (roleId == Shared.Constants.Identificators.RoleID.CompanyAdmin) return AppRole.CompanyAdmin;
            else if (roleId == Shared.Constants.Identificators.RoleID.SysAdmin) return AppRole.SysAdmin;
            else if (roleId == Shared.Constants.Identificators.RoleID.SuperAdmin) return AppRole.SuperAdmin;
            else return null;
        }
    }
}
