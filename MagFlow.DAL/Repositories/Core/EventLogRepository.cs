using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.Core
{
    public class EventLogRepository : BaseCoreRepository<EventLog, EventLogRepository>, IEventLogRepository
    {
        public EventLogRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<EventLogRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
