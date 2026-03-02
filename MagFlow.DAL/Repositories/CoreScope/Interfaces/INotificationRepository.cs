using MagFlow.Domain.CoreScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface INotificationRepository : IRepository<MagFlow.Domain.CoreScope.Notification>
    {
        Task<List<SystemNotification>> GetCurrentSystemNotificationsAsync();
    }
}
