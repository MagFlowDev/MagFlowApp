using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;

namespace MagFlow.Shared.Models
{
    public class SessionCache
    {
        public Guid SessionId { get; set; }
        public List<Guid> SessionOrder { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }

    public class SessionCurrentModule
    {
        public Guid SessionId { get; set; }
        public Guid ModuleId { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }

    public class SessionModuleSection
    {
        public Guid SessionId { get; set; }
        public Guid ModuleId { get; set; }
        public Enum Section { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
