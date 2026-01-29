using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Domain.Core
{
    public class SessionModule
    {
        [Required]
        public Guid SessionId { get; set; }
        [Required]
        public Guid ModuleId { get; set; }

        [ForeignKey("SessionId")]
        public UserSession? Session { get; set; }
        [ForeignKey("ModuleId")]
        public Module? Module { get; set; }
    }
}
