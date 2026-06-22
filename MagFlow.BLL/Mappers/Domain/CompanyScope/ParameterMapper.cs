using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using MudBlazor;

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

        public static ProductParameterDTO ToDTO(this ProductParameterFormModel model)
        {
            return new ProductParameterDTO()
            {
                Name = model.Name
            };
        }

        public static List<ProductParameterDTO> ToDTO(this IEnumerable<ProductParameterFormModel> models)
        {
            return models.Select(x => x.ToDTO()).ToList();
        }



        public static ProductParameter ToEntity(this ProductParameterDTO parameter)
        {
            return new ProductParameter()
            {
                Id = parameter.Id,
                ParameterId = parameter.ParameterId,
            };
        }

        public static List<ProductParameter> ToEntity(this IEnumerable<ProductParameterDTO> parameters)
        {
            return parameters.Select(x => x.ToEntity()).ToList();
        }

        public static ProductParameter ToEntity(this ProductParameterFormModel model)
        {
            return new ProductParameter()
            {
            };
        }

        public static List<ProductParameter> ToEntity(this IEnumerable<ProductParameterFormModel> models)
        {
            return models.Select(x => x.ToEntity()).ToList();
        }




        public static ParameterDTO ToDTO(this CustomParameter parameter)
        {
            return new ParameterDTO()
            {
                Id = parameter.Id,
                Name = parameter.Name,
                Code = parameter.Code,
                ValueType = parameter.ValueType,
                Unit = parameter.Unit?.ToDTO()
            };
        }

        public static List<ParameterDTO> ToDTO(this IEnumerable<CustomParameter> parameters)
        {
            return parameters.Select(x => x.ToDTO()).ToList();
        }

        public static ParameterDTO ToDTO(this ParameterFormModel model)
        {
            return new ParameterDTO()
            {
                Name = model.Name,
                Code = model.Code,
                ValueType = model.ValueType,
                Unit = model.Unit
            };
        }

        public static List<ParameterDTO> ToDTO(this IEnumerable<ParameterFormModel> models)
        {
            return models.Select(x => x.ToDTO()).ToList();
        }



        public static CustomParameter ToEntity(this ParameterDTO parameter)
        {
            return new CustomParameter()
            {
                Id = parameter.Id,
                Name = parameter.Name,
                Code = parameter.Code,
                UnitId = parameter.Unit?.Id,
                ValueType = parameter.ValueType ?? Enums.ValueType.Decimal
            };
        }

        public static List<CustomParameter> ToEntity(this IEnumerable<ParameterDTO> parameters)
        {
            return parameters.Select(x => x.ToEntity()).ToList();
        }

        public static CustomParameter ToEntity(this ParameterFormModel model)
        {
            return new CustomParameter()
            {
                Name = model.Name,
                Code = model.Code,
                UnitId = model.Unit?.Id,
                ValueType = model.ValueType ?? Enums.ValueType.Decimal
            };
        }

        public static List<CustomParameter> ToEntity(this IEnumerable<ParameterFormModel> models)
        {
            return models.Select(x => x.ToEntity()).ToList();
        }

    }
}
