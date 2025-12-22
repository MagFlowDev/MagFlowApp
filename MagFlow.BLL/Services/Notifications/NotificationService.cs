using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Mappers.Domain.Core;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Shared.DTOs.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task<List<NotificationDTO>> GetCurrentSystemNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetCurrentSystemNotificationsAsync();
            return notifications.ToDTO();
        }
    }
}
