using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class CategoryMapper
    {
        public static ProductCategoryDTO ToDTO(this ProductCategory category)
        {
            return new ProductCategoryDTO()
            {
                Id = category.Id
            };
        }
    }
}
