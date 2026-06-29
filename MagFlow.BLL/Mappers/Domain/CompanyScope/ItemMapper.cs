using MagFlow.BLL.Helpers;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class ItemMapper
    {
        public static ItemDTO ToDTO(this Item item)
        {
            var parameters = new List<ItemParameterDTO>();
            if (item.Parameters != null)
            {
                foreach (var parameter in item.Parameters)
                {
                    if (parameter.Parameter == null)
                        continue;
                    var dto = parameter.Parameter.ToDTO();
                    var value = parameter.Value;
                    parameters.Add(new ItemParameterDTO() { Parameter = dto, Value = value });
                }
            }

            return new ItemDTO()
            {
                Id = item.Id,
                ExternalId = item.ExternalId,
                CreatedAt = item.CreatedAt,
                Quantity = item.Quantity,
                Status = item.Status,
                Unit = item.DefaultUnit?.ToDTO(),
                Product = item.Product?.ToDTO(),
                Location = item.Location,
                Parameters = parameters,
            };
        }

        public static List<ItemDTO> ToDTO(this IEnumerable<Item> items)
        {
            return items.Select(x => x.ToDTO()).ToList();
        }



        public static Item ToEntity(this ItemDTO item, Guid? userId = null)
        {
            var parameters = new List<ItemParameter>();
            foreach (var parameter in item.Parameters)
            {
                var value = parameter.Value;
                if (value == null && !parameter.Parameter.IsRequired)
                    continue;
                parameters.Add(new ItemParameter()
                {
                    ParameterId = parameter.Parameter.Id,
                    Value = value
                });
            }

            return new Item()
            {
                ProductId = item.Product?.Id ?? 0,
                Location = item.Location,
                CreatedAt = item.CreatedAt ?? DateTime.UtcNow,
                Quantity = item.Quantity,
                CreatedById = item.CreatedBy?.Id ?? userId ?? Guid.Empty,
                IsBlocked = false,
                Condition = Shared.Models.Enums.Condition.Unknown,
                Status = item.Status,
                DefaultUnitId = item.Unit?.Id ?? 0,
                
                PurchasePrice = item.Product?.PurchasePrice,
                SellPrice = item.Product?.SellingPrice,
                TaxRate = item.Product?.TaxRate?.ToDecimal(),
                Currency = item.Product?.Currency,

                Parameters = parameters
            };
        }

        public static List<Item> ToEntity(this IEnumerable<ItemDTO> items, Guid? userId = null)
        {
            return items.Select(x => x.ToEntity(userId)).ToList();
        }

        public static Item ToEntity(this ItemFormModel model, Guid userId)
        {
            var parameters = new List<ItemParameter>();
            foreach (var parameter in model.ParameterValues.Parameters)
            {
                var value = parameter.Value;
                if (value == null && !parameter.Parameter.IsRequired)
                    continue;
                parameters.Add(new ItemParameter()
                {
                    ParameterId = parameter.Parameter.Id,
                    Value = value
                });
            }

            return new Item()
            {
                ProductId = model.GeneralInformation.Product?.Id ?? 0,
                Location = model.GeneralInformation.Location,
                CreatedAt = DateTime.UtcNow,
                Quantity = model.GeneralInformation.Quantity ?? 0,
                CreatedById = userId,
                IsBlocked = false,
                Condition = Shared.Models.Enums.Condition.Unknown,
                Status = Shared.Models.Enums.ItemStatus.Available,
                DefaultUnitId = model.GeneralInformation.Unit?.Id ?? 0,

                PurchasePrice = model.GeneralInformation.Product?.PurchasePrice,
                SellPrice = model.GeneralInformation.Product?.SellingPrice,
                TaxRate = model.GeneralInformation.Product?.TaxRate?.ToDecimal(),
                Currency = model.GeneralInformation.Product?.Currency,

                Parameters = parameters
            };
        }

        public static List<Item> ToEntity(this IEnumerable<ItemFormModel> models, Guid userId)
        {
            return models.Select(x => x.ToEntity(userId)).ToList();
        }
    }
}
