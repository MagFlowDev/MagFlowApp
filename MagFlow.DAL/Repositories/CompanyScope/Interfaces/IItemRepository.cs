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
    public interface IItemRepository : IRepository<Item, CompanyDbContext>
    {
        Task<Enums.Result> UpdateItemQuantity(Dictionary<int, decimal> itemsQuantity, Enums.ItemStatus removalReason, CompanyDbContext? context = null);
    }
}
