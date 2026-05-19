using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class ParameterMapper
    {
        public static ProductParameterDTO ToDTO(this ProductParameter parameter)
        {
            return new ProductParameterDTO()
            {
                Id = parameter.Id,
                ParameterId = parameter.ParameterId,
                Name = parameter.Parameter?.Name ?? string.Empty,
                Code = parameter.Parameter?.Code ?? string.Empty,
                ValueType = parameter.Parameter?.ValueType,
                Unit = parameter.Parameter?.Unit?.ToDTO()
            };
        }

        public static List<ProductParameterDTO> ToDTO(this IEnumerable<ProductParameter> parameters)
        {
            return parameters.Select(x => x.ToDTO()).ToList();
        }
    }
}
