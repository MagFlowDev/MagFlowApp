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
                Id = type.Id
            };
        }
    }
}
