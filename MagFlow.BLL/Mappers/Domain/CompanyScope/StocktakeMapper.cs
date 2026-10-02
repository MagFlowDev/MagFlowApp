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
            return items.Select(item => StocktakeItem.CreateItemCopy(item)).ToList();
        }

        public static List<StocktakeItemParameter> CreateStocktakeItemParameters(this IEnumerable<ItemParameter> parameters, int itemId = 0)
        {
            return parameters.Select(parameter => StocktakeItemParameter.CreateParameterCopy(parameter, itemId)).ToList();
        }
    }
}
