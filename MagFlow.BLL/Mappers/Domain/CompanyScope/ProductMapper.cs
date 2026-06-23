using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.Models.FormModels;
using MagFlow.BLL.Helpers;
using MagFlow.Shared.Models;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class ProductMapper
    {
        public static ProductDTO ToDTO(this Product product)
        {
            var parameters = new List<ParameterDTO>();
            if (product.Parameters != null)
            {
                foreach (var parameter in product.Parameters)
                {
                    if (parameter.Parameter == null)
                        continue;
                    var dto = parameter.Parameter.ToDTO();
                    dto.IsRequired = parameter.IsRequired;
                    parameters.Add(dto);
                }
            }

            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Status = product.IsActive ? Enums.ProductStatus.Active : Enums.ProductStatus.Inactive,
                Type = product.Type?.ToDTO(),
                Category = product.Category?.ToDTO(),
                Unit = product.Unit?.ToDTO(),
                PurchasePrice = product.DefaultPurchasePrice,
                SellingPrice = product.DefaultSellPrice,
                TaxRate = EnumsHelper.ToTaxRate(product.DefaultVatRate),
                Currency = product.Currency,
                Parameters = parameters
            };
        }

        public static List<ProductDTO> ToDTO(this IEnumerable<Product> products)
        {
            return products.Select(x => x.ToDTO()).ToList();
        }

        public static ProductDTO ToDTO(this ProductFormModel model)
        {
            var parameters = new List<ParameterDTO>();
            foreach (var parameter in model.Parameters.Parameters)
            {
                parameters.Add(new ParameterDTO()
                {
                    Code = parameter.Code,
                    Name = parameter.Name,
                    ValueType = parameter.ValueType,
                    Unit = parameter.Unit,
                    IsRequired = parameter.IsRequired,
                });
            }

            return new ProductDTO()
            {
                Name = model.GeneralInformation.Name,
                Code = model.GeneralInformation.Code,
                Status = Enums.ProductStatus.Active,
                Type = model.GeneralInformation.ProductType,
                Category = model.GeneralInformation.ProductCategory,
                Unit = model.GeneralInformation.Unit,
                PurchasePrice = model.Prices.PurchasePrice,
                SellingPrice = model.Prices.SellingPrice,
                TaxRate = model.Prices.TaxRate,
                Currency = model.Prices.Currency,
                Parameters = parameters
            };
        }

        public static List<ProductDTO> ToDTO(this IEnumerable<ProductFormModel> models)
        {
            return models.Select(x => x.ToDTO()).ToList();
        }



        public static Product ToEntity(this ProductDTO product, Guid? userId = null)
        {
            var parameters = new List<ProductParameter>();
            foreach (var parameter in product.Parameters)
            {
                parameters.Add(new ProductParameter()
                {
                    ParameterId = parameter.Id,
                    IsRequired = parameter.IsRequired
                });
            }

            return new Product()
            {
                Name = product.Name,
                Code = product.Code,
                CreatedAt = product.CreatedAt ?? DateTime.UtcNow,
                CreatedById = product.CreatedBy?.Id ?? userId ?? Guid.Empty,
                IsActive = product.Status == Enums.ProductStatus.Active,
                TypeId = product.Type?.Id ?? 0,
                CategoryId = product.Category?.Id,
                UnitId = product.Unit?.Id ?? 0,

                DefaultPurchasePrice = product.PurchasePrice,
                DefaultSellPrice = product.SellingPrice,
                DefaultVatRate = product.TaxRate?.ToDecimal(),
                Currency = product.Currency,

                Parameters = parameters
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
                UnitId = model.GeneralInformation.Unit?.Id ?? 0,

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
