using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class StockMovementMapper
    {
        public static StockMovementDTO ToDTO(this StockMovement stockMovement)
        {
            return new StockMovementDTO()
            {
                SourceLocation = new WarehouseLocationDTO()
                {
                    Warehouse = stockMovement.SourceWarehouse == null ? null : new WarehouseDTO()
                    {
                        Id = stockMovement.SourceWarehouse.Id,
                        Name = stockMovement.SourceWarehouse.Name,
                    },
                    Sector = stockMovement.SourceSector == null ? null : new SectorDTO()
                    {
                        Id = stockMovement.SourceSector.Id,
                        Name = stockMovement.SourceSector.Name,
                    },
                    Row = stockMovement.SourceRow == null ? null : new RowDTO()
                    {
                        Id = stockMovement.SourceRow.Id,
                        Name = stockMovement.SourceRow.Name,
                    },
                    Slot = stockMovement.SourceSlot == null ? null : new SlotDTO()
                    {
                        Id = stockMovement.SourceSlot.Id,
                        Name = stockMovement.SourceSlot.Name,
                    },
                },
                TargetLocation = new WarehouseLocationDTO()
                {
                    Warehouse = stockMovement.TargetWarehouse == null ? null : new WarehouseDTO()
                    {
                        Id = stockMovement.TargetWarehouse.Id,
                        Name = stockMovement.TargetWarehouse.Name,
                    },
                    Sector = stockMovement.TargetSector == null ? null : new SectorDTO()
                    {
                        Id = stockMovement.TargetSector.Id,
                        Name = stockMovement.TargetSector.Name,
                    },
                    Row = stockMovement.TargetRow == null ? null : new RowDTO()
                    {
                        Id = stockMovement.TargetRow.Id,
                        Name = stockMovement.TargetRow.Name,
                    },
                    Slot = stockMovement.TargetSlot == null ? null : new SlotDTO()
                    {
                        Id = stockMovement.TargetSlot.Id,
                        Name = stockMovement.TargetSlot.Name,
                    },
                }
            };
        }

        public static List<StockMovementDTO> ToDTO(this IEnumerable<StockMovement> stockMovements)
        {
            return stockMovements.Select(x => x.ToDTO()).ToList();
        }
    }
}
