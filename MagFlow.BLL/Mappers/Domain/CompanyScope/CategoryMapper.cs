using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models.FormModels;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class CategoryMapper
    {
        public static ProductCategoryDTO ToDTO(this ProductCategory category)
        {
            return new ProductCategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                Code = category.Code,
                IsActive = category.IsActive,
                IsBasic = category.IsBasic
            };
        }

        public static List<ProductCategoryDTO> ToDTO(this IEnumerable<ProductCategory> categories)
        {
            return categories.Select(x => x.ToDTO()).ToList();
        }

        public static ProductCategoryDTO ToDTO(this ProductCategoryFormModel model)
        {
            return new ProductCategoryDTO()
            {
                Name = model.Name,
                Code = model.Code,
                IsBasic = model.IsBasic
            };
        }

        public static List<ProductCategoryDTO> ToDTO(this IEnumerable<ProductCategoryFormModel> models)
        {
            return models.Select(x => x.ToDTO()).ToList();
        }




        public static ProductCategory ToEntity(this ProductCategoryDTO type)
        {
            return new ProductCategory()
            {
                Id = type.Id,
                Name = type.Name,
                Code = type.Code,
                IsActive = type.IsActive,
                IsBasic = type.IsBasic
            };
        }

        public static List<ProductCategory> ToEntity(this IEnumerable<ProductCategoryDTO> types)
        {
            return types.Select(x => x.ToEntity()).ToList();
        }

        public static ProductCategory ToEntity(this ProductCategoryFormModel model)
        {
            return new ProductCategory()
            {
                Name = model.Name,
                Code = model.Code,
                IsActive = true,
                IsBasic = model.IsBasic,
            };
        }

        public static List<ProductCategory> ToEntity(this IEnumerable<ProductCategoryFormModel> models)
        {
            return models.Select(x => x.ToEntity()).ToList();
        }
    }
}
