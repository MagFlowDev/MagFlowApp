using MagFlow.Domain.CoreScope;
using MagFlow.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CoreScope.Interfaces
{
    public interface IModuleRepository : IRepository<Module, CoreDbContext>
    {
    }
}
