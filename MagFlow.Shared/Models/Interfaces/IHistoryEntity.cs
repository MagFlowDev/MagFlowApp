using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Shared.Models.Interfaces
{
    public interface IEntityHistory 
    {
        int Id { get; set; }
        int EntityId { get; set; }
        Enums.HistoryEntityType EntityType { get; set; }
        string? EventType { get; set; }
        DateTime OccurredAt { get; set; }
        string? OldValuesJson { get; set; }
        string? NewValuesJson { get; set; }
        Guid? UserId { get; set; }

        IUser? User { get; }
    }

    public interface IHistoryEntity
    {
        int Id { get; }
        Enums.HistoryEntityType EntityType { get; }

        [NotMapped]
        ICollection<IEntityHistory> History { get; set; }
    }
}
