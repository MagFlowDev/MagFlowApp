using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.DTOs.CoreScope
{
    public class CompanyDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string DbName { get; set; }
        public string TaxNumber { get; set; } 
        public DateTime CreatedAt { get; set; }
        public Address? Address { get; set; }
        public bool IsActive { get; set; }
        public byte[]? LogoData { get; set; }
        public string? LogoContentType { get; set; }
        public CompanySettingsDTO? CompanySettings { get; set; }
        public List<CompanyModuleDTO>? CompanyModules { get; set; }
    }
}
