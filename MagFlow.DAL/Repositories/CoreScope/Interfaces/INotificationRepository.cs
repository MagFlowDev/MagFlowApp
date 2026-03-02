using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface INotificationRepository : IRepository<Notification, CoreDbContext>
    {
        Task<List<SystemNotification>> GetCurrentSystemNotificationsAsync();
    }
}
