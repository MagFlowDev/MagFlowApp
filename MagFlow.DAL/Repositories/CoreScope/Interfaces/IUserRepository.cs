using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser, CoreDbContext>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<List<UserSession>?> GetLastSessionsAsync(Guid userId, int historyLength = 1);
        Task<QueryResponse<ApplicationUser>?> GetCompanyUsersAsync(QueryOptions queryOptions);

        
        
        Task<Enums.Result> UpdateSettingsAsync(ApplicationUserSettings settings);
    }
}
