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
        Task<Dictionary<int, (int Total, int Checked)>> GetStocktakeCounts(IReadOnlyCollection<int> stocktakeIds);
        Task<Enums.Result> FinishStocktake(int stocktakeId, Guid userId);
        Task<Enums.Result> ResumeStocktake(int stocktakeId, Guid userId);
        Task<Enums.Result> ApproveStocktake(int stocktakeId, Guid userId);
        Task<Enums.Result> DeleteStocktake(int stocktakeId, Guid userId);
        Task<Enums.Result> CancelStocktake(int stocktakeId, Guid userId);
        Task<Enums.Result> SaveStocktakeItem(int stocktakeId, int itemId,
            StocktakeItemValues? values, Enums.StocktakeItemCheckStatus? mark, Guid userId);
        Task<Enums.Result> PrepareStocktake(StocktakeFormModel model, Guid userId);
    }
}
