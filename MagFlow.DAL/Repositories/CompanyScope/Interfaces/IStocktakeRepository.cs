using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope.Interfaces
{
    public interface IStocktakeRepository : IRepository<Stocktake, CompanyDbContext>
    {
        Task<Enums.Result> PrepareStocktake(StocktakeFormModel model, Guid userId);
    }
}
