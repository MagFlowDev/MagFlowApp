using MagFlow.Shared.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Helpers.Auth
{
    public class RoleAuthorizationHelper
    {
        public static bool HasMinimumRole(string minimum, List<string> roles)
        {
            try
            {
                var minimumRole = Enumeration<Guid>.GetAll<AppRole>().FirstOrDefault(x => x.Name == minimum);
                if (minimumRole == null)
                    return true;
                var userRoles = Enumeration<Guid>.GetAll<AppRole>().Where(x => roles.Contains(x.Name)).ToList();

                List<AppRole> requiredRoles = new List<AppRole>();

                if (minimumRole == AppRole.Foreman)
                    requiredRoles = new List<AppRole> { AppRole.Foreman, AppRole.Operator, AppRole.Supervisor, AppRole.Auditor, AppRole.CompanyAdmin, AppRole.SysAdmin, AppRole.SuperAdmin };
                else if (minimumRole == AppRole.Operator)
                    requiredRoles = new List<AppRole> { AppRole.Operator, AppRole.Supervisor, AppRole.Auditor, AppRole.CompanyAdmin, AppRole.SysAdmin, AppRole.SuperAdmin };
                else if (minimumRole == AppRole.Supervisor)
                    requiredRoles = new List<AppRole> { AppRole.Supervisor, AppRole.Auditor, AppRole.CompanyAdmin, AppRole.SysAdmin, AppRole.SuperAdmin };
                else if (minimumRole == AppRole.Auditor)
                    requiredRoles = new List<AppRole> { AppRole.Auditor, AppRole.CompanyAdmin, AppRole.SysAdmin, AppRole.SuperAdmin };
                else if (minimumRole == AppRole.CompanyAdmin)
                    requiredRoles = new List<AppRole> { AppRole.CompanyAdmin, AppRole.SysAdmin, AppRole.SuperAdmin };
                else if (minimumRole == AppRole.SysAdmin)
                    requiredRoles = new List<AppRole> { AppRole.SysAdmin, AppRole.SuperAdmin };
                else if (minimumRole == AppRole.SuperAdmin)
                    requiredRoles = new List<AppRole> { AppRole.SuperAdmin };
                else
                    return true;

                if (requiredRoles.Intersect(userRoles).Any())
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool HasExactRole(string required, List<string> roles)
        {
            try
            {
                var requiredRole = Enumeration<Guid>.GetAll<AppRole>().FirstOrDefault(x => x.Name == required);
                if (requiredRole == null)
                    return true;
                var userRoles = Enumeration<Guid>.GetAll<AppRole>().Where(x => roles.Contains(x.Name)).ToList();

                if (userRoles.Contains(requiredRole))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
