using MagFlow.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.Core.Interfaces
{
    public interface INotificationRepository : IRepository<MagFlow.Domain.Core.Notification>
    {
        Task<List<SystemNotification>> GetCurrentSystemNotificationsAsync();
    }
}
