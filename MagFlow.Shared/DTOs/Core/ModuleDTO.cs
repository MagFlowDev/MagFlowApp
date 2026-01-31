using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagFlow.Shared.Models.Enumerators;

namespace MagFlow.Shared.DTOs.Core
{
    public class ModuleDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ModuleType? Type { get; set; }

        public IModule? ModuleComponent { get; set; }
    }
}
