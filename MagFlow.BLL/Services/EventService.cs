using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CoreScope;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagFlow.BLL.Mappers.Domain.CoreScope;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly ILogger<EventService> _logger;

        public EventService(IEventLogRepository eventLogRepository,
            ILogger<EventService> logger)
        {
            _eventLogRepository = eventLogRepository;
            _logger = logger;
        }

        public async Task AddEventAsync(Guid userId, Enums.EventLogCategory category, Enums.EventLogLevel level, string message, string details, string ip, string agent)
        {
            var now = DateTime.UtcNow;
            EventLog eventLog = new EventLog()
            {
                UserId = userId,
                Category = category, 
                Level = level,
                Message = message,
                Details = details,
                IpAddress = ip,
                UserAgent = agent,
                OccuredAt = now
            };
            await _eventLogRepository.AddAsync(eventLog);
        }

        public async Task<QueryResponse<EventLogDTO>> GetUserEvents(Guid userId, int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _eventLogRepository.GetAsync(new QueryOptions<EventLog>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new System.Linq.Expressions.Expression<Func<EventLog, string?>>[]
                {
                    s => s.Details, s => s.OccuredAt.ToString()
                },
                Filters = new Dictionary<string, object>()
                {
                    { nameof(EventLog.UserId), userId }
                },
                SortBy = sortBy,
                Descending = descending
            });
            return new QueryResponse<EventLogDTO>()
            {
                Elements = queryResponse?.Elements
                    .Select(x => x.ToDTO())
                    .ToList() ?? new List<EventLogDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }
    }
}
