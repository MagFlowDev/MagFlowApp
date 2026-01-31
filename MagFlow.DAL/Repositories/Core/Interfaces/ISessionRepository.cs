using MagFlow.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagFlow.Shared.Models;

namespace MagFlow.DAL.Repositories.Core.Interfaces
{
    public interface ISessionRepository : IRepository<UserSession>
    {
        Task<List<Module>?> GetSessionModules(Guid sessionId);
    }
}
