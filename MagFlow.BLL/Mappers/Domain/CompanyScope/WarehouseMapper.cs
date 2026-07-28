using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class WarehouseMapper
    {
        #region Warehouse

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
                Sectors = warehouse.Sectors?.ToDTO() ?? new List<SectorDTO>(),
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

        #endregion


        #region Sector

        public static SectorDTO ToDTO(this WarehouseSector sector)
        {
            if (sector == null)
                return null;
            return new SectorDTO
            {
                Id = sector.Id,
                Name = sector.Name,
                Code = sector.Code,
                Status = sector.Status,
                CreatedAt = sector.CreatedAt,
                RemovedAt = sector.RemovedAt,
                Items = sector.Items?.ToDTO() ?? new List<ItemDTO>(),
                Rows = sector.Rows?.ToDTO() ?? new List<RowDTO>(),
            };
        }

        public static List<SectorDTO> ToDTO(this ICollection<WarehouseSector> sectors)
        {
            return sectors.Select(x => x.ToDTO()).ToList();
        }





        public static WarehouseSector ToEntity(this SectorDTO sectorDTO)
        {
            if (sectorDTO == null)
                return null;
            return new WarehouseSector
            {
                Id = sectorDTO.Id,
                WarehouseId = sectorDTO.WarehouseId,
                Name = sectorDTO.Name,
                Code = sectorDTO.Code,
                Status = sectorDTO.Status,
                CreatedAt = sectorDTO.CreatedAt ?? DateTime.UtcNow,
                RemovedAt = sectorDTO.RemovedAt,
            };
        }

        public static List<WarehouseSector> ToEntity(this ICollection<SectorDTO> sectorDTOs)
        {
            return sectorDTOs.Select(x => x.ToEntity()).ToList();
        }

        #endregion


        #region Row

        public static RowDTO ToDTO(this WarehouseSectorRow row)
        {
            if (row == null)
                return null;
            return new RowDTO
            {
                Id = row.Id,
                Name = row.Name,
                Code = row.Code,
                Status = row.Status,
                CreatedAt = row.CreatedAt,
                RemovedAt = row.RemovedAt,
                Items = row.Items?.ToDTO() ?? new List<ItemDTO>(),
                Slots = row.Slots?.ToDTO() ?? new List<SlotDTO>(),
            };
        }

        public static List<RowDTO> ToDTO(this ICollection<WarehouseSectorRow> rows)
        {
            return rows.Select(x => x.ToDTO()).ToList();
        }





        public static WarehouseSectorRow ToEntity(this RowDTO rowDTO)
        {
            if (rowDTO == null)
                return null;
            return new WarehouseSectorRow
            {
                Id = rowDTO.Id,
                SectorId = rowDTO.SectorId,
                Name = rowDTO.Name,
                Code = rowDTO.Code,
                Status = rowDTO.Status,
                CreatedAt = rowDTO.CreatedAt ?? DateTime.UtcNow,
                RemovedAt = rowDTO.RemovedAt,
            };
        }

        public static List<WarehouseSectorRow> ToEntity(this ICollection<RowDTO> rowDTOs)
        {
            return rowDTOs.Select(x => x.ToEntity()).ToList();
        }

        #endregion


        #region Slot

        public static SlotDTO ToDTO(this WarehouseSectorRowSlot slot)
        {
            if (slot == null)
                return null;
            return new SlotDTO
            {
                Id = slot.Id,
                Name = slot.Name,
                Code = slot.Code,
                Status = slot.Status,
                CreatedAt = slot.CreatedAt,
                RemovedAt = slot.RemovedAt,
                Items = slot.Items?.ToDTO() ?? new List<ItemDTO>(),
            };
        }

        public static List<SlotDTO> ToDTO(this ICollection<WarehouseSectorRowSlot> slots)
        {
            return slots.Select(x => x.ToDTO()).ToList();
        }





        public static WarehouseSectorRowSlot ToEntity(this SlotDTO slotDTO)
        {
            if (slotDTO == null)
                return null;
            return new WarehouseSectorRowSlot
            {
                Id = slotDTO.Id,
                RowId = slotDTO.RowId,
                Name = slotDTO.Name,
                Code = slotDTO.Code,
                Status = slotDTO.Status,
                CreatedAt = slotDTO.CreatedAt ?? DateTime.UtcNow,
                RemovedAt = slotDTO.RemovedAt,
            };
        }

        public static List<WarehouseSectorRowSlot> ToEntity(this ICollection<SlotDTO> slotDTOs)
        {
            return slotDTOs.Select(x => x.ToEntity()).ToList();
        }

        #endregion
    }
}
