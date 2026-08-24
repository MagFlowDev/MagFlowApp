using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using static Grpc.Core.Metadata;

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
                    case nameof(WarehouseSector):
                        return (TDTO)(object)WarehouseMapper.ToDTO((WarehouseSector)(object)entity);
                    case nameof(WarehouseSectorRow):
                        return (TDTO)(object)WarehouseMapper.ToDTO((WarehouseSectorRow)(object)entity);
                    case nameof(WarehouseSectorRowSlot):
                        return (TDTO)(object)WarehouseMapper.ToDTO((WarehouseSectorRowSlot)(object)entity);
                    case nameof(Contractor):
                        return (TDTO)(object)ContractorMapper.ToDTO((Contractor)(object)entity);
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



        public static TEntity ToEntity<TEntity, TDTO>(this TDTO dto, Guid? createdById = null)
        {
            if (dto == null)
                return default(TEntity);

            try
            {
                createdById ??= Guid.Empty;
                var dtoType = typeof(TDTO);
                switch (dtoType.Name)
                {
                    case nameof(ProductDTO):
                        return (TEntity)(object)ProductMapper.ToEntity((ProductDTO)(object)dto);
                    case nameof(ItemDTO):
                        return (TEntity)(object)ItemMapper.ToEntity((ItemDTO)(object)dto);
                    case nameof(WarehouseDTO):
                        return (TEntity)(object)WarehouseMapper.ToEntity((WarehouseDTO)(object)dto, createdById.Value);
                    case nameof(SectorDTO):
                        return (TEntity)(object)WarehouseMapper.ToEntity((SectorDTO)(object)dto, createdById.Value);
                    case nameof(RowDTO):
                        return (TEntity)(object)WarehouseMapper.ToEntity((RowDTO)(object)dto, createdById.Value);
                    case nameof(SlotDTO):
                        return (TEntity)(object)WarehouseMapper.ToEntity((SlotDTO)(object)dto, createdById.Value);
                    case nameof(ContractorDTO):
                        return (TEntity)(object)ContractorMapper.ToEntity((ContractorDTO)(object)dto);
                    default:
                        return default(TEntity);
                }
            }
            catch(Exception ex)
            {
                return default(TEntity);
            }
        }

        public static List<TEntity> ToEntity<TEntity, TDTO>(this ICollection<TDTO> dtos)
        {
            return dtos.Select(x => x.ToEntity<TEntity, TDTO>()).ToList();
        }
    }
}
