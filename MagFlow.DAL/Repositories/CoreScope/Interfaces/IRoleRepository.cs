using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface IRoleRepository : IRepository<ApplicationRole, CoreDbContext>
    {
        
    }
}
