using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Constants.Identificators
{
    public class RoleID
    {
        public static Guid Foreman = new Guid("2dda161b-f7ba-4235-bfea-be81c95a8957");
        public static Guid Operator = new Guid("5101b30d-6b6f-45ad-92dc-8c07087d3b02");
        public static Guid Supervisor = new Guid("abe1534f-7e8a-44a6-b8e6-e8e239c062bc");
        public static Guid Auditor = new Guid("1c84b5ed-7b29-46cc-ad82-49881d1c4442");
        public static Guid CompanyAdmin = new Guid("b03362f2-091a-4823-8e84-836e96eb7edc");
        public static Guid SysAdmin = new Guid("4062cec6-e42d-4ab4-b898-56d297af0dea");
        public static Guid SuperAdmin = new Guid("a8add820-71ba-40dc-877d-a8198ae2f8b0");
    }
}
