using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IEventService
    {
        Task AddEventAsync(Guid userId, Enums.EventLogCategory category, Enums.EventLogLevel level, string message, string details, string ip, string agent);
        Task<QueryResponse<EventLogDTO>> GetUserEvents(Guid userId, int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
    }
}
