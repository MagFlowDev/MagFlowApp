using MagFlow.DAL.Helpers;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Enums.Result> UpdateItemQuantity(Dictionary<int, decimal> itemsQuantity, Enums.ItemStatus removalReason, CompanyDbContext? context = null)
        {
            try
            {
                var now = DateTime.UtcNow;


                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        var itemsIds = itemsQuantity.Keys.ToArray();

                        var quantityCase = QueryHelper.BuildCaseExpression<Item, int, decimal>(
                            keyPropertyName: nameof(Item.Id),
                            targetPropertyName: nameof(Item.Quantity),
                            updates: itemsQuantity
                        );

                        await context.Items
                            .Where(item => itemsIds.Contains(item.Id))
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(item => item.Quantity, quantityCase));

                        var removedItemsDict = itemsQuantity
                        .Where(kvp => kvp.Value == 0m)
                        .ToDictionary(kvp => kvp.Key, kvp => (DateTime?)now);
                        var removedItemsDict2 = itemsQuantity
                            .Where(kvp => kvp.Value == 0m)
                            .ToDictionary(kvp => kvp.Key, kvp => Enums.ItemStatus.Used);

                        if (removedItemsDict.Any())
                        {
                            var removedItemsIds = removedItemsDict.Keys.ToArray();

                            var removedAtCase = QueryHelper.BuildCaseExpression<Item, int, DateTime?>(
                               keyPropertyName: nameof(Item.Id),
                               targetPropertyName: nameof(Item.RemovedAt),
                               updates: removedItemsDict
                            );
                            var removedAtCase2 = QueryHelper.BuildCaseExpression<Item, int, Enums.ItemStatus>(
                               keyPropertyName: nameof(Item.Id),
                               targetPropertyName: nameof(Item.Status),
                               updates: removedItemsDict2
                            );

                            await context.Items
                                .Where(item => removedItemsIds.Contains(item.Id))
                                .ExecuteUpdateAsync(setters => setters
                                    .SetProperty(item => item.RemovedAt, removedAtCase)
                                    .SetProperty(item => item.Status, removedAtCase2));
                        }
                    }
                }
                else
                {
                    var itemsIds = itemsQuantity.Keys.ToArray();

                    var quantityCase = QueryHelper.BuildCaseExpression<Item, int, decimal>(
                        keyPropertyName: nameof(Item.Id),
                        targetPropertyName: nameof(Item.Quantity),
                        updates: itemsQuantity
                    );

                    await context.Items
                        .Where(item => itemsIds.Contains(item.Id))
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(item => item.Quantity, quantityCase));

                    var removedItemsDict = itemsQuantity
                        .Where(kvp => kvp.Value == 0m)
                        .ToDictionary(kvp => kvp.Key, kvp => (DateTime?)now);
                    var removedItemsDict2 = itemsQuantity
                        .Where(kvp => kvp.Value == 0m)
                        .ToDictionary(kvp => kvp.Key, kvp => removalReason);

                    if (removedItemsDict.Any())
                    {
                        var removedItemsIds = removedItemsDict.Keys.ToArray();

                        var removedAtCase = QueryHelper.BuildCaseExpression<Item, int, DateTime?>(
                           keyPropertyName: nameof(Item.Id),
                           targetPropertyName: nameof(Item.RemovedAt),
                           updates: removedItemsDict
                        );
                        var removedAtCase2 = QueryHelper.BuildCaseExpression<Item, int, Enums.ItemStatus>(
                           keyPropertyName: nameof(Item.Id),
                           targetPropertyName: nameof(Item.Status),
                           updates: removedItemsDict2
                        );

                        await context.Items
                            .Where(item => removedItemsIds.Contains(item.Id))
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(item => item.RemovedAt, removedAtCase)
                                .SetProperty(item => item.Status, removedAtCase2));
                    }

                }
                return Enums.Result.Success;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
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
