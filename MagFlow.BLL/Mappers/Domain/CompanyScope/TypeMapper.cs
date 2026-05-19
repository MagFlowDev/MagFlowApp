using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class TypeMapper
    {
        public static ProductTypeDTO ToDTO(this ProductType type)
        {
            return new ProductTypeDTO()
            {
                Id = type.Id,
                Name = type.Name,
                Code = type.Code,
                IsActive = type.IsActive,
                IsBasic = type.IsBasic
            };
        }

        public static List<ProductTypeDTO> ToDTO(this IEnumerable<ProductType> types)
        {
            return types.Select(x => x.ToDTO()).ToList();
        }
    }
}
