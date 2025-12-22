using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.DTOs.Core
{
    public class CompanyDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; } 
    }
}
