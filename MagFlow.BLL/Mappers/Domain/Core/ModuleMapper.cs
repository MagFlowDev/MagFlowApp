using MagFlow.Domain.Core;
using MagFlow.Shared.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Domain.Core
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
                Name = module.Name
            };
        }

        public static List<ModuleDTO> ToDTO(this IEnumerable<Module> modules)
        {
            return modules.Where(x => x.IsActive).Select(x => ToDTO(x)!).ToList();
        }
    }
}
