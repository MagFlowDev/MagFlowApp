using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagFlow.Shared.Models;

namespace MagFlow.DAL.Repositories.Core
{
    public class SessionRepository : BaseCoreRepository<MagFlow.Domain.Core.UserSession, SessionRepository>, ISessionRepository
    {
        public SessionRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<SessionRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {

        }

        public async Task<List<Module>?> GetSessionModules(Guid sessionId)
        {
            try
            {

                using (var context = _coreContextFactory.CreateDbContext())
                {
                    var session = await context.UserSessions
                        .Include(x => x.SessionModules).ThenInclude(y => y.Module)
                        .FirstOrDefaultAsync(x => x.Id == sessionId);

                    if (session == null)
                    {
                        _logger.LogWarning($"Session with ID {sessionId} not found.");
                        return null;
                    }
                    return session.SessionModules?
                        .Where(x => x.Module != null && x.Module.IsActive)
                        .Select(x => x.Module!)
                        .ToList();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving session modules for session ID {sessionId}");
                return null;
            }
        }
    }
}
