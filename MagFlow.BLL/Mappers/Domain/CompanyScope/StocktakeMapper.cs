using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class StocktakeMapper
    {
        public static StocktakeDetailsDTO ToDetailsDTO(this Stocktake stocktake)
        {
            return new StocktakeDetailsDTO
            {
                Stocktake = stocktake.ToDTO(),
                Items = stocktake.Items.OrderBy(x => x.Id).Select(item =>
                {
                    var original = item.OriginalValuesJson == null ? item.GetValues()
                        : System.Text.Json.JsonSerializer.Deserialize<MagFlow.Shared.Models.FormModels.StocktakeItemValues>(
                            item.OriginalValuesJson) ?? item.GetValues();
                    return new StocktakeItemDTO
                    {
                        Id = item.Id, Code = item.Code,
                        ProductStatus = item.Product?.Status ?? Enums.EntityStatus.Unknown,
                        OriginalQuantity = original.Quantity,
                        ProductName = item.Product?.Name ?? string.Empty,
                        ProductType = item.Product?.Type?.Name ?? string.Empty,
                        Category = item.Product?.Category?.Name ?? string.Empty,
                        Unit = item.DefaultUnit?.Symbol ?? string.Empty,
                        LocationName = string.Join(" / ", new[]
                        {
                            item.Warehouse?.Name, item.Sector?.Name, item.Row?.Name, item.Slot?.Name
                        }.Where(x => !string.IsNullOrEmpty(x))),
                        CheckStatus = item.CheckStatus, Values = item.GetValues(),
                        Parameters = item.Parameters.OrderBy(x => x.Id).Select(parameter => new StocktakeParameterDTO
                        {
                            Id = parameter.Id, Name = parameter.Parameter?.Name ?? string.Empty,
                            OriginalValue = original.Parameters.TryGetValue(parameter.Id, out var value)
                                ? value : parameter.Value,
                            Unit = parameter.Parameter?.Unit?.Symbol ?? string.Empty,
                            ValueType = parameter.Parameter?.ValueType ?? Enums.ValueType.Text
                        }).ToList()
                    };
                }).ToList()
            };
        }

        public static StocktakeDTO ToDTO(this Stocktake stocktake)
        {
            return new StocktakeDTO
            {
                Id = stocktake.Id,
                Code = stocktake.Code,
                TotalItems = stocktake.Items.Count,
                CheckedItems = stocktake.Items.Count(x => x.CheckStatus == Enums.StocktakeItemCheckStatus.Checked),
                WarehouseId = stocktake.WarehouseId,
                WarehouseName = stocktake.Warehouse?.Name ?? string.Empty,
                Type = stocktake.Type,
                Status = stocktake.Status,
                PlannedDate = stocktake.PlannedDate,
                CreatedAt = stocktake.CreatedAt,
                ConfirmedAt = stocktake.ConfirmedAt,
                CreatedByName = stocktake.CreatedBy == null ? string.Empty
                    : $"{stocktake.CreatedBy.FirstName} {stocktake.CreatedBy.LastName}"
            };
        }

        public static Stocktake CreateStocktake(this WarehouseDTO warehouse, Enums.StocktakeType type, DateTime plannedDate, Guid createdById)
        {
            return new Stocktake()
            {
                WarehouseId = warehouse.Id,
                Type = type,
                PlannedDate = plannedDate,
                CreatedAt = DateTime.UtcNow,
                CreatedById = createdById
            };
        }

        public static List<StocktakeItem> CreateStocktakeItems(this IEnumerable<Item> items, int stocktakeId = 0)
        {
            return items.Select(item => StocktakeItem.CreateItemCopy(item, stocktakeId)).ToList();
        }

        public static List<StocktakeItemParameter> CreateStocktakeItemParameters(this IEnumerable<ItemParameter> parameters, int itemId = 0)
        {
            return parameters.Select(parameter => StocktakeItemParameter.CreateParameterCopy(parameter, itemId)).ToList();
        }
    }
}
