using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope.Interfaces
{
    public interface IContractorRepository : IRepository<Contractor, CompanyDbContext>
    {
        Task<Enums.Result> ChangeStatuses(IReadOnlyCollection<int> ids, Enums.EntityStatus status, Guid userId);
        Task<Enums.Result> RestoreArchivedContractors(IReadOnlyCollection<int> ids);
    }
}
