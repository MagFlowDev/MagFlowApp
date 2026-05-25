using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.Models.FormModels;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class ProductMapper
    {
        public static ProductDTO ToDTO(this Product product)
        {
            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
            };
        }

        public static List<ProductDTO> ToDTO(this IEnumerable<Product> products)
        {
            return products.Select(x => x.ToDTO()).ToList();
        }

        public static ProductDTO ToDTO(this ProductFormModel model)
        {
            return new ProductDTO()
            {

            };
        }

        public static List<ProductDTO> ToDTO(this IEnumerable<ProductFormModel> models)
        {
            return models.Select(x => x.ToDTO()).ToList();
        }



        public static Product ToEntity(this ProductDTO type)
        {
            return new Product()
            {

            };
        }

        public static List<Product> ToEntity(this IEnumerable<ProductDTO> types)
        {
            return types.Select(x => x.ToEntity()).ToList();
        }

        public static Product ToEntity(this ProductFormModel model)
        {
            return new Product()
            {

            };
        }

        public static List<Product> ToEntity(this IEnumerable<ProductFormModel> models)
        {
            return models.Select(x => x.ToEntity()).ToList();
        }
    }
}
