using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.Models.FormModels;
using MagFlow.BLL.Helpers;

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



        public static Product ToEntity(this ProductDTO product)
        {
            return new Product()
            {

            };
        }

        public static List<Product> ToEntity(this IEnumerable<ProductDTO> products)
        {
            return products.Select(x => x.ToEntity()).ToList();
        }

        public static Product ToEntity(this ProductFormModel model, Guid userId)
        {
            var parameters = new List<ProductParameter>();
            foreach(var parameter in model.Parameters.Parameters)
            {
                parameters.Add(new ProductParameter()
                {
                    ParameterId = parameter.Id,
                    IsRequired = false
                });
            }

            return new Product()
            {
                Name = model.GeneralInformation.Name,
                Code = model.GeneralInformation.Code,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId,
                IsActive = true,
                TypeId = model.GeneralInformation.ProductType?.Id ?? 0,
                CategoryId = model.GeneralInformation.ProductCategory?.Id,

                DefaultPurchasePrice = model.Prices.PurchasePrice,
                DefaultSellPrice = model.Prices.SellingPrice,
                DefaultVatRate = model.Prices.TaxRate?.ToDecimal(),
                Currency = model.Prices.Currency,

                Parameters = parameters
            };
        }

        public static List<Product> ToEntity(this IEnumerable<ProductFormModel> models, Guid userId)
        {
            return models.Select(x => x.ToEntity(userId)).ToList();
        }
    }
}
