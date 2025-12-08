using MagFlow.Shared.Constants;
using MagFlow.Shared.Constants.Identificators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.Enumerators
{
    public class AppRole : Enumeration<Guid>
    {
        public static AppRole Foreman = new(RoleID.Foreman, nameof(Foreman)); // Użytkownik operacyjny
        public static AppRole Operator = new(RoleID.Operator, nameof(Operator)); // Użytkownik wykonawczy
        public static AppRole Supervisor = new(RoleID.Supervisor, nameof(Supervisor)); // Użytkownik decyzyjny
        public static AppRole Auditor = new(RoleID.Auditor, nameof(Auditor)); // Użytkownik kontrolujący
        public static AppRole CompanyAdmin = new(RoleID.CompanyAdmin, nameof(CompanyAdmin)); // Administrator firmy
        public static AppRole SysAdmin = new(RoleID.SysAdmin, nameof(SysAdmin)); // Administrator systemu
        public static AppRole SuperAdmin = new(RoleID.SuperAdmin, nameof(SuperAdmin)); // Super administrator

        public AppRole(Guid id, string name) : base(id, name)
        {
        }
    }
}
