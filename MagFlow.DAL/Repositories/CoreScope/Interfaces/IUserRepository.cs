using MagFlow.Domain.CompanyScope;
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
        Task<List<UserSession>?> GetLastSessionsAsync(Guid userId, Guid companyId, int historyLength = 1);
        Task<QueryResponse<ApplicationUser>?> GetCompanyUsersAsync(QueryOptions<User> queryOptions);

        
        
        Task<Enums.Result> UpdateSettingsAsync(ApplicationUserSettings settings);
    }
}
