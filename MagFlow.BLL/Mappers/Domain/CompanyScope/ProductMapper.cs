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
            var components = new List<ComponentDTO>();
            if (product.Components != null)
            {
                foreach (var component in product.Components)
                {
                    if (component.Component == null)
                        continue;
                    var dto = component.Component.ToSimpleDTO();
                    components.Add(new ComponentDTO()
                    {
                        Product = dto,
                        IsRequired = component.IsRequired,
                        Note = component.Note,
                        Quantity = component.Quantity
                    });
                }
            }
            var unitConversions = new List<UnitConversionDTO>();
            if (product.Conversions != null)
            {
                foreach(var conversion in product.Conversions)
                {
                    if (conversion.FromUnit == null || conversion.ToUnit == null)
                        continue;
                    var fromUnitDTO = conversion.FromUnit.ToDTO();
                    var toUnitDTO = conversion.ToUnit.ToDTO();
                    unitConversions.Add(new UnitConversionDTO()
                    {
                        Id = conversion.Id,
                        FromUnit = fromUnitDTO,
                        ToUnit = toUnitDTO,
                        ConversionRate = conversion.ConversionRate,
                        Note = conversion.Note,
                    });
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
                CreatedAt = product.CreatedAt,
                CreatedBy = new Shared.DTOs.CoreScope.UserDTO() { Id = product.CreatedById },
                PurchasePrice = product.DefaultPurchasePrice,
                SellingPrice = product.DefaultSellPrice,
                TaxRate = EnumsHelper.ToTaxRate(product.DefaultVatRate),
                Currency = product.Currency,
                Parameters = parameters,
                Components = components,
                UnitConversions = unitConversions,
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
            var components = new List<ComponentDTO>();
            foreach (var component in model.Components.Components)
            {
                components.Add(new ComponentDTO()
                {
                    Product = component.Product,
                    Quantity = component.Quantity,
                    IsRequired = true
                });
            }
            var unitConversions = new List<UnitConversionDTO>();
            foreach (var conversion in model.UnitConversions.Conversions)
            {
                if(conversion != null)
                    unitConversions.Add(conversion);
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
                Parameters = parameters,
                Components = components,
                UnitConversions = unitConversions
            };
        }

        public static List<ProductDTO> ToDTO(this IEnumerable<ProductFormModel> models)
        {
            return models.Select(x => x.ToDTO()).ToList();
        }

        public static ProductDTO ToSimpleDTO(this ProductFormModel model)
        {
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
                Parameters = new List<ParameterDTO>(),
                Components = new List<ComponentDTO>(),
                UnitConversions = new List<UnitConversionDTO>()
            };
        }

        public static List<ProductDTO> ToSimpleDTO(this IEnumerable<ProductFormModel> models)
        {
            return models.Select(x => x.ToSimpleDTO()).ToList();
        }

        public static ProductDTO ToSimpleDTO(this Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Status = product.IsActive
           ? Enums.ProductStatus.Active
           : Enums.ProductStatus.Inactive,
                Type = product.Type?.ToDTO(),
                Category = product.Category?.ToDTO(),
                Unit = product.Unit?.ToDTO(),
                PurchasePrice = product.DefaultPurchasePrice,
                SellingPrice = product.DefaultSellPrice,
                TaxRate = EnumsHelper.ToTaxRate(product.DefaultVatRate),
                Currency = product.Currency,
                Parameters = new List<ParameterDTO>(),
                Components = new List<ComponentDTO>(),
                UnitConversions = new List<UnitConversionDTO>()
            };
        }

        public static List<ProductDTO> ToSimpleDTO(this IEnumerable<Product> products)
        {
            return products.Select(x => x.ToSimpleDTO()).ToList();
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
            var components = new List<ProductComponent>();
            foreach (var component in product.Components)
            {
                components.Add(new ProductComponent()
                {
                    ComponentId = component.Product.Id,
                    Quantity = component.Quantity,
                    IsRequired = true,
                });
            }
            var unitConversions = new List<ProductUnitConversion>();
            foreach(var conversion in product.UnitConversions)
            {
                unitConversions.Add(new ProductUnitConversion()
                {
                    Id = conversion.Id ?? 0,
                    FromUnitId = conversion.FromUnit.Id,
                    ToUnitId = conversion.ToUnit.Id,
                    Note = conversion.Note,
                    ConversionRate = conversion.ConversionRate,
                });
            }

            var productToAdd = new Product()
            {
                Id = product.Id,
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

                Parameters = parameters,
                Components = components,
                Conversions = unitConversions
            };

            return productToAdd;
        }

        public static List<Product> ToEntity(this IEnumerable<ProductDTO> products, Guid? userId = null)
        {
            return products.Select(x => x.ToEntity(userId)).ToList();
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
            var components = new List<ProductComponent>();
            foreach (var component in model.Components.Components)
            {
                components.Add(new ProductComponent()
                {
                    ComponentId = component.Product.Id,
                    Quantity = component.Quantity,
                    IsRequired = true,
                });
            }
            var unitConversions = new List<ProductUnitConversion>();
            foreach (var conversion in model.UnitConversions.Conversions)
            {
                unitConversions.Add(new ProductUnitConversion()
                {
                    Id = conversion.Id ?? 0,
                    FromUnitId = conversion.FromUnit.Id,
                    ToUnitId = conversion.ToUnit.Id,
                    Note = conversion.Note,
                    ConversionRate = conversion.ConversionRate,
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

                Parameters = parameters,
                Components = components,
                Conversions = unitConversions
            };
        }

        public static List<Product> ToEntity(this IEnumerable<ProductFormModel> models, Guid userId)
        {
            return models.Select(x => x.ToEntity(userId)).ToList();
        }
    }
}
