using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class WarehouseMapper
    {
        #region Warehouse

        public static WarehouseDTO ToDTO(this Warehouse warehouse, bool mapItems = false)
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
                Items = mapItems ? warehouse.Items?.ToDTO() ?? new List<ItemDTO>() : new List<ItemDTO>(),
                Sectors = warehouse.Sectors?.ToDTO(mapItems) ?? new List<SectorDTO>(),
                StockMovements = warehouse.StockMovements.ToDTO(),
            };
        }

        public static List<WarehouseDTO> ToDTO(this ICollection<Warehouse> warehouses, bool mapItems = false)
        {
            return warehouses.Select(x => x.ToDTO(mapItems)).ToList();
        }





        public static Warehouse ToEntity(this WarehouseDTO warehouseDTO, Guid createdById)
        {
            if (warehouseDTO == null)
                return null;

            var sectors = warehouseDTO.Sectors.ToEntity(createdById);
            return new Warehouse
            {
                Id = warehouseDTO.Id,
                Name = warehouseDTO.Name,
                Description = warehouseDTO.Description,
                IsActive = warehouseDTO.IsActive,
                Type = warehouseDTO.Type,
                Status = warehouseDTO.Status,
                CreatedAt = warehouseDTO.CreatedAt ?? DateTime.UtcNow,
                CreatedById = createdById,
                RemovedAt = warehouseDTO.RemovedAt,
                Items = warehouseDTO.Items?.ToEntity() ?? new List<Item>(),
                Sectors = sectors
            };
        }

        public static List<Warehouse> ToEntity(this ICollection<WarehouseDTO> warehouseDTOs, Guid createdById)
        {
            return warehouseDTOs.Select(x => x.ToEntity(createdById)).ToList();
        }

        public static Warehouse ToEntity(this WarehouseFormModel model, Guid createdById)
        {
            if (model == null)
                return null;
            var now = DateTime.UtcNow;

            var sectors = model.Sectors.ToEntity(createdById);
            return new Warehouse()
            {
                Name = model.GeneralInformation.Name,
                Type = model.GeneralInformation.Type,
                CreatedAt = now,
                CreatedById = createdById,
                Status = Shared.Models.Enums.EntityStatus.Active,
                Sectors = sectors
            };
        }

        #endregion


        #region Sector

        public static SectorDTO ToDTO(this WarehouseSector sector, bool mapItems = false)
        {
            if (sector == null)
                return null;
            return new SectorDTO
            {
                Id = sector.Id,
                WarehouseId = sector.WarehouseId,
                Name = sector.Name,
                Code = sector.Code,
                Status = sector.Status,
                CreatedAt = sector.CreatedAt,
                RemovedAt = sector.RemovedAt,
                Items = mapItems ? sector.Items?.ToDTO() ?? new List<ItemDTO>() : new List<ItemDTO>(),
                Rows = sector.Rows?.ToDTO(mapItems) ?? new List<RowDTO>(),
            };
        }

        public static List<SectorDTO> ToDTO(this ICollection<WarehouseSector> sectors, bool mapItems = false)
        {
            return sectors.Select(x => x.ToDTO(mapItems)).ToList();
        }





        public static WarehouseSector ToEntity(this SectorDTO sectorDTO, Guid createdById)
        {
            if (sectorDTO == null)
                return null;
            return new WarehouseSector
            {
                Id = sectorDTO.Id,
                WarehouseId = sectorDTO.WarehouseId,
                Name = sectorDTO.Name,
                Status = sectorDTO.Status,
                CreatedAt = sectorDTO.CreatedAt ?? DateTime.UtcNow,
                RemovedAt = sectorDTO.RemovedAt,
                CreatedById = createdById,
                Rows = sectorDTO.Rows.ToEntity(createdById)
            };
        }

        public static List<WarehouseSector> ToEntity(this ICollection<SectorDTO> sectorDTOs, Guid createdById)
        {
            return sectorDTOs.Select(x => x.ToEntity(createdById)).ToList();
        }

        #endregion


        #region Row

        public static RowDTO ToDTO(this WarehouseSectorRow row, bool mapItems = false)
        {
            if (row == null)
                return null;
            return new RowDTO
            {
                Id = row.Id,
                SectorId = row.SectorId,
                Name = row.Name,
                Code = row.Code,
                Status = row.Status,
                CreatedAt = row.CreatedAt,
                RemovedAt = row.RemovedAt,
                Items = mapItems ? row.Items?.ToDTO() ?? new List<ItemDTO>() : new List<ItemDTO>(),
                Slots = row.Slots?.ToDTO(mapItems) ?? new List<SlotDTO>(),
            };
        }

        public static List<RowDTO> ToDTO(this ICollection<WarehouseSectorRow> rows, bool mapItems = false)
        {
            return rows.Select(x => x.ToDTO(mapItems)).ToList();
        }





        public static WarehouseSectorRow ToEntity(this RowDTO rowDTO, Guid createdById)
        {
            if (rowDTO == null)
                return null;
            return new WarehouseSectorRow
            {
                Id = rowDTO.Id,
                SectorId = rowDTO.SectorId,
                Name = rowDTO.Name,
                Status = rowDTO.Status,
                CreatedAt = rowDTO.CreatedAt ?? DateTime.UtcNow,
                RemovedAt = rowDTO.RemovedAt,
                CreatedById = createdById,
                Slots = rowDTO.Slots.ToEntity(createdById)
            };
        }

        public static List<WarehouseSectorRow> ToEntity(this ICollection<RowDTO> rowDTOs, Guid createdById)
        {
            return rowDTOs.Select(x => x.ToEntity(createdById)).ToList();
        }

        #endregion


        #region Slot

        public static SlotDTO ToDTO(this WarehouseSectorRowSlot slot, bool mapItems = false)
        {
            if (slot == null)
                return null;
            return new SlotDTO
            {
                Id = slot.Id,
                RowId = slot.RowId,
                Name = slot.Name,
                Code = slot.Code,
                Status = slot.Status,
                CreatedAt = slot.CreatedAt,
                RemovedAt = slot.RemovedAt,
                Items = mapItems ? slot.Items?.ToDTO() ?? new List<ItemDTO>() : new List<ItemDTO>(),
            };
        }

        public static List<SlotDTO> ToDTO(this ICollection<WarehouseSectorRowSlot> slots, bool mapItems = false)
        {
            return slots.Select(x => x.ToDTO(mapItems)).ToList();
        }





        public static WarehouseSectorRowSlot ToEntity(this SlotDTO slotDTO, Guid createdById)
        {
            if (slotDTO == null)
                return null;
            return new WarehouseSectorRowSlot
            {
                Id = slotDTO.Id,
                RowId = slotDTO.RowId,
                Name = slotDTO.Name,
                Status = slotDTO.Status,
                CreatedAt = slotDTO.CreatedAt ?? DateTime.UtcNow,
                RemovedAt = slotDTO.RemovedAt,
                CreatedById = createdById,
            };
        }

        public static List<WarehouseSectorRowSlot> ToEntity(this ICollection<SlotDTO> slotDTOs, Guid createdById)
        {
            return slotDTOs.Select(x => x.ToEntity(createdById)).ToList();
        }

        #endregion
    }
}
