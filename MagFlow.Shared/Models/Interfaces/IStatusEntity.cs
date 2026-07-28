using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.Interfaces
{
    public interface IStatusEntity
    {
        Enums.EntityStatus Status { get; }
        IReadOnlyCollection<Enums.EntityStatus> AllowedStatuses { get; }
        void ChangeStatus(Enums.EntityStatus newStatus);
    }
}
