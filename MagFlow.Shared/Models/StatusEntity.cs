using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Shared.Models
{
    public abstract class StatusEntity : HistoryEntity, IStatusEntity
    {
        protected Enums.EntityStatus _status;

        [Required]
        public Enums.EntityStatus Status { get => _status; init => _status = value; }

        [NotMapped]
        public IReadOnlyCollection<Enums.EntityStatus> AllowedStatuses { get; }

        public void ChangeStatus(Enums.EntityStatus newStatus)
        {
            if (newStatus == Enums.EntityStatus.Deleted && !AllowedStatuses.Contains(newStatus))
                newStatus = Enums.EntityStatus.Inactive;
            else if (newStatus == Enums.EntityStatus.Available && !AllowedStatuses.Contains(newStatus))
                newStatus = Enums.EntityStatus.Active;

            if (!AllowedStatuses.Contains(newStatus))
                throw new ArgumentException($"Status '{newStatus}' is not allowed for entity type: {this.GetType().Name}");

            _status = newStatus;
        }

        protected StatusEntity()
        {
            AllowedStatuses = Array.Empty<Enums.EntityStatus>();
        }

        public StatusEntity(IReadOnlyCollection<Enums.EntityStatus> allowedStatuses)
        {
            AllowedStatuses = allowedStatuses ?? Array.Empty<Enums.EntityStatus>();
        }
    }
}
