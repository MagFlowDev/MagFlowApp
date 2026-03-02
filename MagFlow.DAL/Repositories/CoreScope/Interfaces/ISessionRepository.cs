using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface ISessionRepository : IRepository<UserSession, CoreDbContext>
    {
        Task<List<Module>?> GetSessionModules(Guid sessionId);
        Task<Enums.Result> RemoveSessionModulesAsync(List<SessionModule> modules);
        Task<Enums.Result> AddSessionModulesAsync(List<SessionModule> modules);
    }
}
