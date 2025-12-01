using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.Core
{
    public class NotificationRepository : BaseCoreRepository<MagFlow.Domain.Core.Notification, NotificationRepository>, INotificationRepository
    {
        public NotificationRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<NotificationRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {

        }

        public async Task<List<SystemNotification>> GetCurrentSystemNotificationsAsync()
        {
            try
            {
                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var now = DateTime.UtcNow;
                    return await context.SystemNotifications
                        .Where(x => x.ExpireAt > now && x.IsArchived == false)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new List<SystemNotification>();
            }
        }
    }
}
