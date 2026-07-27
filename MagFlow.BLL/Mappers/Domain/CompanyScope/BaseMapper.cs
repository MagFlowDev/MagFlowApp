using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class BaseMapper
    {
        public static TDTO ToDTO<TDTO, TEntity>(this TEntity entity)
        {
            if(entity == null)
                return default(TDTO);

            try
            {
                var entityType = typeof(TEntity);
                switch (entityType.Name)
                {
                    case nameof(Product):
                        return (TDTO)(object)ProductMapper.ToDTO((Product)(object)entity);
                    case nameof(Item):
                        return (TDTO)(object)ItemMapper.ToDTO((Item)(object)entity);
                    case nameof(Warehouse):
                        return (TDTO)(object)WarehouseMapper.ToDTO((Warehouse)(object)entity);
                    default:
                        return default(TDTO);
                }
            }
            catch (Exception ex)
            {
                return default(TDTO);
            }
        }

        public static List<TDTO> ToDTO<TDTO, TEntity>(this ICollection<TEntity> entities)
        {
            return entities.Select(x => x.ToDTO<TDTO, TEntity>()).ToList();
        }
    }
}
