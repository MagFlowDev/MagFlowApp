using MagFlow.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.Core.Interfaces
{
    public interface IUserRepository : IRepository<MagFlow.Domain.Core.ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);

        Task<UserSession?> GetLastSessionAsync(Guid userId);
    }
}
