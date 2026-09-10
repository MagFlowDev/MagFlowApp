using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Helpers
{
    public static class StockMovementsFactory
    {
        public static void TryAddLocationChangeMovement(this Item updatedItem,
            Item originalItem,
            Guid userId,
            Enums.StockMovementType movementType = Enums.StockMovementType.InternalMove,
            int? documentId = null)
        {
            updatedItem.StockMovements ??= new List<StockMovement>();

            bool warehouseChanged = originalItem.WarehouseId != updatedItem.WarehouseId;
            bool sectorChanged = originalItem.SectorId != updatedItem.SectorId;
            bool rowChanged = originalItem.RowId != updatedItem.RowId;
            bool slotChanged = originalItem.SlotId != updatedItem.SlotId;

            if (!warehouseChanged && !sectorChanged && !rowChanged && !slotChanged)
                return;

            var movement = new StockMovement
            {
                Quantity = updatedItem.Quantity,
                MovementType = movementType,

                SourceWarehouseId = originalItem.WarehouseId,
                SourceSectorId = originalItem.SectorId,
                SourceRowId = originalItem.RowId,
                SourceSlotId = originalItem.SlotId,

                TargetWarehouseId = updatedItem.WarehouseId,
                TargetSectorId = updatedItem.SectorId,
                TargetRowId = updatedItem.RowId,
                TargetSlotId = updatedItem.SlotId,

                CreatedAt = DateTime.UtcNow,
                CreatedById = userId,
                DocumentId = documentId
            };

            updatedItem.StockMovements.Add(movement);
        }
    }
}
