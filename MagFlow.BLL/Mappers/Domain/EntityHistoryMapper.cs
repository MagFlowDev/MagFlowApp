using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain
{
    public static class EntityHistoryMapper
    {
        public static EntityHistoryDTO ToDTO(this EntityHistory entityHistory)
        {
            UserDTO? user = null;
            return new EntityHistoryDTO()
            {
                Id = entityHistory.Id,
                EventType = entityHistory.EventType,
                OccurredAt = entityHistory.OccurredAt,
                OldValuesJson = entityHistory.OldValuesJson,
                NewValuesJson = entityHistory.NewValuesJson,
                User = user
            };
        }

        public static EntityHistoryDTO ToDTO(this IEntityHistory entityHistory)
        {
            return new EntityHistoryDTO()
            {
                Id = entityHistory.Id,
                EventType = entityHistory.EventType,
                OccurredAt = entityHistory.OccurredAt,
                OldValuesJson = entityHistory.OldValuesJson,
                NewValuesJson = entityHistory.NewValuesJson,
                User = null
            };
        }

        public static List<EntityHistoryDTO> ToDTO(this IEnumerable<EntityHistory> entityHistories)
        {
            return entityHistories.Select(x => x.ToDTO()).ToList();
        }

        public static List<EntityHistoryDTO> ToDTO(this IEnumerable<IEntityHistory> entityHistories)
        {
            return entityHistories.Select(x => x.ToDTO()).ToList();
        }
    }
}
