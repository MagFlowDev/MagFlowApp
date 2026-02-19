using MagFlow.Domain.Core;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.Core.Interfaces
{
    public interface IUserRepository : IRepository<MagFlow.Domain.Core.ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);

        Task<List<UserSession>?> GetLastSessionsAsync(Guid userId, int historyLength = 1);

        Task<Enums.Result> UpdateSettingsAsync(ApplicationUserSettings settings);
    }
}
