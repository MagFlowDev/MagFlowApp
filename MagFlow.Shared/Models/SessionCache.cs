using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models
{
    public class SessionCache
    {
        public Guid SessionId { get; set; }
        public List<Guid> SessionOrder { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
