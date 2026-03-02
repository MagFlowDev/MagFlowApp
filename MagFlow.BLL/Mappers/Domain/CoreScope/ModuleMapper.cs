using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagFlow.Shared.Models.Enumerators;

namespace MagFlow.BLL.Mappers.Domain.CoreScope
{
    public static class ModuleMapper
    {
        public static ModuleDTO? ToDTO(this Module module)
        {
            if(module.IsActive == false)
                return null;

            return new ModuleDTO()
            {
                Id = module.Id,
                Code = module.Code,
                Description = module.Description,
                Name = module.Name,
                Type = ModuleType.GetModuleType(module.Name)
            };
        }

        public static List<ModuleDTO> ToDTO(this IEnumerable<Module> modules)
        {
            return modules.Where(x => x.IsActive).Select(x => ToDTO(x)!).ToList();
        }

    }
}
