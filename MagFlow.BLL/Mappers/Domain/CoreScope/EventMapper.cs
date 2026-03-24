using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CoreScope
{
    public static class EventMapper
    {
        public static EventLogDTO ToDTO(this EventLog eventLog)
        {
            return new EventLogDTO()
            {
                Id = eventLog.Id,
                IpAddress = eventLog.IpAddress,
                OccuredAt = eventLog.OccuredAt,
                Category = eventLog.Category,
                Level = eventLog.Level,
                Details = eventLog.Details,
                Message = eventLog.Message,
            };
        }

        public static List<EventLogDTO> ToDTO(this IEnumerable<EventLog> eventLogs)
        {
            return eventLogs.Select(x => ToDTO(x)).ToList();
        }
    }
}
