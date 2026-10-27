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
        public static StocktakeDTO ToDTO(this Stocktake stocktake)
        {
            return new StocktakeDTO
            {
                Id = stocktake.Id,
                WarehouseId = stocktake.WarehouseId,
                WarehouseName = stocktake.Warehouse?.Name ?? string.Empty,
                Type = stocktake.Type,
                Status = stocktake.Status,
                PlannedDate = stocktake.PlannedDate,
                CreatedAt = stocktake.CreatedAt,
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
