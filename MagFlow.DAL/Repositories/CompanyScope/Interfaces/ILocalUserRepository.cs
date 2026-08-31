using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models.Domain.CompanyScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope.Interfaces
{
    public interface ILocalUserRepository : IRepository<User, CompanyDbContext>
    {
    }
}
