using MagFlow.Shared.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CoreScope
{
    public class CompanyModuleDTO
    {
        public Guid ModuleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime EnabledFrom { get; set; }
        public DateTime EnabledTo { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool IsActive { get; set; }
        public string? Icon { get; set; }
        public ModuleType? Type { get; set; }
    }
}
