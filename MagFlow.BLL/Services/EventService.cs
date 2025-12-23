using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
        }
    }
}
