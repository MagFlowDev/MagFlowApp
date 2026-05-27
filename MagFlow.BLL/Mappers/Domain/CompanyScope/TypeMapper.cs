using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.Models.FormModels;

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

        public static ProductTypeDTO ToDTO(this ProductTypeFormModel model, int id = 0)
        {
            return new ProductTypeDTO()
            {
                Id = id,
                Code = model.Code,
                Name = model.Name,
                IsBasic = model.IsBasic
            };
        }

        public static List<ProductTypeDTO> ToDTO(this IEnumerable<ProductTypeFormModel> models)
        {
            return models.Select(x => x.ToDTO()).ToList();
        }



        public static ProductType ToEntity(this ProductTypeDTO type)
        {
            return new ProductType()
            {
                Id = type.Id,
                Name = type.Name,
                Code = type.Code,
                IsActive = type.IsActive,
                IsBasic = type.IsBasic
            };
        }

        public static List<ProductType> ToEntity(this IEnumerable<ProductTypeDTO> types)
        { 
            return types.Select(x => x.ToEntity()).ToList();
        }

        public static ProductType ToEntity(this ProductTypeFormModel model)
        {
            return new ProductType()
            {
                Name = model.Name,
                Code = model.Code,
                IsBasic = model.IsBasic,
                IsActive = true
            };
        }

        public static List<ProductType> ToEntity(this IEnumerable<ProductTypeFormModel> models)
        {
            return models.Select(x => x.ToEntity()).ToList();
        }
    }
}
