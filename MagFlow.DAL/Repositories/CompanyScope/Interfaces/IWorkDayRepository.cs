using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope.Interfaces
{
    public interface IWorkDayRepository : IRepository<WorkDay, CompanyDbContext>
    {
    }
}
