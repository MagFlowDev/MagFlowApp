using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class ItemRepository : BaseCompanyRepository<Item, ItemRepository>, IItemRepository
    {
        public ItemRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<ItemRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }

        public override async Task<Enums.Result> DeleteAsync(Item entity, CompanyDbContext? context = null)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        entity.RemovedAt = DateTime.UtcNow;
                        entity.Status = Enums.ItemStatus.Deleted;
                        context.Items.Update(entity);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    entity.RemovedAt = DateTime.UtcNow;
                    entity.Status = Enums.ItemStatus.Deleted;
                    context.Items.Update(entity);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public override async Task<Enums.Result> DeleteManyAsync(Expression<Func<Item, bool>> predicate, CompanyDbContext? context = null)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        var entities = Find(predicate, context);
                        if (entities == null)
                            return Enums.Result.Error;
                        foreach (var entity in entities)
                        {
                            entity.RemovedAt = DateTime.UtcNow;
                            entity.Status = Enums.ItemStatus.Deleted;
                        }
                        context.Items.UpdateRange(entities);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var entities = Find(predicate, context);
                    if (entities == null)
                        return Enums.Result.Error;
                    foreach (var entity in entities)
                    {
                        entity.RemovedAt = DateTime.UtcNow;
                        entity.Status = Enums.ItemStatus.Deleted;
                    }
                    context.Items.UpdateRange(entities);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }
    }
}
