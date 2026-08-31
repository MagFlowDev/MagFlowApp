using MagFlow.Shared.Models.Domain.CompanyScope;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static MudBlazor.CategoryTypes;

namespace MagFlow.Shared.Models
{
    public abstract class HistoryEntity : IHistoryEntity
    {
        public abstract int Id { get; set; }

        [NotMapped]
        public abstract Enums.HistoryEntityType EntityType { get; }

        [NotMapped]
        public virtual ICollection<IEntityHistory> History { get; set; } = [];

        public void AddHistory<TEntity>(IEntityHistory entity, Guid userId)
        {
            if (!typeof(IHistoryEntity).IsAssignableFrom(typeof(TEntity)))
                return;

            var historyEntityType = ToHistoryEntityType(typeof(TEntity));
            if (historyEntityType != Enums.HistoryEntityType.Unknown)
            {
                this.History.Add(new EntityHistory()
                {
                    EntityType = historyEntityType,
                    OccurredAt = DateTime.UtcNow,
                    EventType = Shared.Constants.HistoryEventTypes.ENTITY_CREATED,
                    UserId = userId,
                });
            }
        }

        public static Enums.HistoryEntityType ToHistoryEntityType(Type entityType)
        {
            switch (entityType.Name)
            {
                case "Product":
                    return Enums.HistoryEntityType.Product;
                case "Item":
                    return Enums.HistoryEntityType.Item;
                case "Warehouse":
                    return Enums.HistoryEntityType.Warehouse;
                case "WarehouseSector":
                    return Enums.HistoryEntityType.WarehouseSector;
                case "WarehouseSectorRow":
                    return Enums.HistoryEntityType.WarehouseSectorRow;
                case "WarehouseSectorRowSlot":
                    return Enums.HistoryEntityType.WarehouseSectorRowSlot;
                case "User":
                    return Enums.HistoryEntityType.User;
                case "Contractor":
                    return Enums.HistoryEntityType.Contractor;
                case "Document":
                    return Enums.HistoryEntityType.Document;
                case "Order":
                    return Enums.HistoryEntityType.Order;
                case "Process":
                    return Enums.HistoryEntityType.Process;
                case "ProcessStep":
                    return Enums.HistoryEntityType.ProcessStep;
                default:
                    return Enums.HistoryEntityType.Unknown;
            }
        }
    }
}
