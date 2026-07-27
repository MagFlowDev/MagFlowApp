using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class WarehouseMapper
    {
        public static WarehouseDTO ToDTO(this Warehouse warehouse)
        {
            if (warehouse == null)
                return null;
            return new WarehouseDTO
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Code = warehouse.Code,
                Description = warehouse.Description,
                IsActive = warehouse.IsActive,
                Type = warehouse.Type,
                Status = warehouse.Status,
                CreatedAt = warehouse.CreatedAt,
                RemovedAt = warehouse.RemovedAt,
                Items = warehouse.Items?.ToDTO() ?? new List<ItemDTO>(),
            };
        }

        public static List<WarehouseDTO> ToDTO(this ICollection<Warehouse> warehouses)
        {
            return warehouses.Select(x => x.ToDTO()).ToList();
        }





        public static Warehouse ToEntity(this WarehouseDTO warehouseDTO)
        {
            if (warehouseDTO == null)
                return null;
            return new Warehouse
            {
                Id = warehouseDTO.Id,
                Name = warehouseDTO.Name,
                Code = warehouseDTO.Code,
                Description = warehouseDTO.Description,
                IsActive = warehouseDTO.IsActive,
                Type = warehouseDTO.Type,
                Status = warehouseDTO.Status,
                CreatedAt = warehouseDTO.CreatedAt ?? DateTime.UtcNow,
                RemovedAt = warehouseDTO.RemovedAt,
                Items = warehouseDTO.Items?.ToEntity() ?? new List<Item>(),
            };
        }

        public static List<Warehouse> ToEntity(this ICollection<WarehouseDTO> warehouseDTOs)
        {
            return warehouseDTOs.Select(x => x.ToEntity()).ToList();
        }
    }
}
