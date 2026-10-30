using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Domain.CompanyScope;
using MagFlow.Shared.Models.FormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class StocktakeRepository : BaseCompanyRepository<Stocktake, StocktakeRepository>, IStocktakeRepository
    {
        public StocktakeRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<StocktakeRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }

        public async Task<Enums.Result> PrepareStocktake(StocktakeFormModel model, Guid userId)
        {
            if (model == null || model.WarehouseId <= 0 ||
                model.Type is not { } type || !Enum.IsDefined(type) ||
                model.PlannedDate is not { } plannedDate ||
                plannedDate.Date == DateTime.MinValue.Date || model.SelectedSectorIds == null)
            {
                return Enums.Result.Error;
            }

            var warehouseId = model.WarehouseId;
            var sectorIds = model.SelectedSectorIds.Distinct().ToArray();
            var includeItemsWithoutSector = model.IncludeItemsWithoutSector;
            if (sectorIds.Any(id => id <= 0) || (sectorIds.Length == 0 && !includeItemsWithoutSector))
            {
                return Enums.Result.Error;
            }

            try
            {
                using var context = _companyContextFactory.CreateDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync(
                    System.Data.IsolationLevel.Serializable);

                if (!await context.Warehouses.AnyAsync(x => x.Id == warehouseId && x.RemovedAt == null))
                {
                    return Enums.Result.Error;
                }

                var matchingSectors = await context.WarehouseSectors.CountAsync(x =>
                    sectorIds.Contains(x.Id) && x.WarehouseId == warehouseId && x.RemovedAt == null);
                if (matchingSectors != sectorIds.Length)
                {
                    return Enums.Result.Error;
                }

                var items = await context.Items.AsNoTracking()
                    .Include(x => x.Parameters)
                    .Where(x => x.RemovedAt == null && x.WarehouseId == warehouseId &&
                        ((x.SectorId.HasValue && sectorIds.Contains(x.SectorId.Value)) ||
                         (includeItemsWithoutSector && x.SectorId == null && x.RowId == null && x.SlotId == null)))
                    .ToListAsync();

                var stocktake = new Stocktake
                {
                    WarehouseId = warehouseId,
                    Type = type,
                    PlannedDate = plannedDate.Date,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = userId
                };
                stocktake.ChangeStatus(Enums.EntityStatus.Prepared);
                context.Stocktakes.Add(stocktake);
                await context.SaveChangesAsync();

                context.StocktakeItems.AddRange(items.Select(item =>
                    StocktakeItem.CreateItemCopy(item, stocktake.Id)));
                context.EntitiesHistory.Add(new EntityHistory
                {
                    EntityId = stocktake.Id,
                    EntityType = Enums.HistoryEntityType.Stocktake,
                    UserId = userId,
                    OccurredAt = stocktake.CreatedAt,
                    EventType = MagFlow.Shared.Constants.HistoryEventTypes.ENTITY_CREATED
                });
                await context.SaveChangesAsync();
                foreach (var copy in context.ChangeTracker.Entries<StocktakeItem>().Select(x => x.Entity))
                {
                    copy.OriginalValuesJson = copy.SerializeValues();
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing stocktake for warehouse {WarehouseId}.", warehouseId);
                return Enums.Result.Error;
            }
        }

        public async Task<Dictionary<int, (int Total, int Checked)>> GetStocktakeCounts(
            IReadOnlyCollection<int> stocktakeIds)
        {
            var ids = stocktakeIds.Distinct().ToArray();
            if (ids.Length == 0)
            {
                return [];
            }
            using var context = _companyContextFactory.CreateDbContext();
            var counts = await context.StocktakeItems.AsNoTracking()
                .Where(x => ids.Contains(x.StocktakeId))
                .GroupBy(x => x.StocktakeId)
                .Select(group => new
                {
                    Id = group.Key,
                    Total = group.Count(),
                    Checked = group.Count(x => x.CheckStatus == Enums.StocktakeItemCheckStatus.Checked)
                }).ToListAsync();
            return counts.ToDictionary(x => x.Id, x => (x.Total, x.Checked));
        }

        public Task<Enums.Result> FinishStocktake(int stocktakeId, Guid userId)
            => ChangeStage(stocktakeId, Enums.EntityStatus.AwaitingApproval, userId);

        public Task<Enums.Result> ResumeStocktake(int stocktakeId, Guid userId)
            => ChangeStage(stocktakeId, Enums.EntityStatus.InProgress, userId);

        public Task<Enums.Result> ApproveStocktake(int stocktakeId, Guid userId)
            => ChangeStage(stocktakeId, Enums.EntityStatus.Confirmed, userId);

        public Task<Enums.Result> DeleteStocktake(int stocktakeId, Guid userId)
            => ChangeStage(stocktakeId, Enums.EntityStatus.Deleted, userId);

        public Task<Enums.Result> CancelStocktake(int stocktakeId, Guid userId)
            => ChangeStage(stocktakeId, Enums.EntityStatus.Canceled, userId);

        private async Task<Enums.Result> ChangeStage(int stocktakeId, Enums.EntityStatus target, Guid userId)
        {
            if (stocktakeId <= 0)
            {
                return Enums.Result.Error;
            }
            try
            {
                using var context = _companyContextFactory.CreateDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync(
                    System.Data.IsolationLevel.Serializable);
                var stocktake = await context.Stocktakes.SingleOrDefaultAsync(x =>
                    x.Id == stocktakeId && x.RemovedAt == null);
                if (stocktake == null)
                {
                    return Enums.Result.Error;
                }
                var oldStatus = stocktake.Status;
                var oldRemovedAt = stocktake.RemovedAt;
                var oldRemovedById = stocktake.RemovedById;
                var allowed = target switch
                {
                    Enums.EntityStatus.AwaitingApproval => stocktake.IsEditable,
                    Enums.EntityStatus.InProgress or Enums.EntityStatus.Confirmed =>
                        oldStatus == Enums.EntityStatus.AwaitingApproval,
                    Enums.EntityStatus.Deleted => oldStatus == Enums.EntityStatus.Prepared,
                    Enums.EntityStatus.Canceled => oldStatus is Enums.EntityStatus.InProgress
                        or Enums.EntityStatus.AwaitingApproval,
                    _ => false
                };
                if (!allowed)
                {
                    return Enums.Result.Error;
                }
                if (target == Enums.EntityStatus.AwaitingApproval)
                {
                    await context.StocktakeItems.Where(x => x.StocktakeId == stocktakeId &&
                        x.CheckStatus == Enums.StocktakeItemCheckStatus.Unchecked)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CheckStatus,
                            Enums.StocktakeItemCheckStatus.Missing));
                }
                var now = DateTime.UtcNow;
                stocktake.ChangeStatus(target);
                if (target == Enums.EntityStatus.Confirmed)
                {
                    stocktake.ConfirmedAt = now;
                }
                if (target == Enums.EntityStatus.Deleted)
                {
                    stocktake.RemovedAt = now;
                    stocktake.RemovedById = userId;
                }
                context.EntitiesHistory.Add(new EntityHistory
                {
                    EntityId = stocktakeId, EntityType = Enums.HistoryEntityType.Stocktake,
                    UserId = userId, OccurredAt = now,
                    EventType = MagFlow.Shared.Constants.HistoryEventTypes.ENTITY_UPDATED,
                    OldValuesJson = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        Status = oldStatus, RemovedAt = oldRemovedAt, RemovedById = oldRemovedById
                    }),
                    NewValuesJson = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        Status = target, stocktake.RemovedAt, stocktake.RemovedById
                    })
                });
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing stage for stocktake {StocktakeId}.", stocktakeId);
                return Enums.Result.Error;
            }
        }

        public async Task<Enums.Result> SaveStocktakeItem(int stocktakeId, int itemId,
            StocktakeItemValues? values, Enums.StocktakeItemCheckStatus? mark, Guid userId)
        {
            if (stocktakeId <= 0 || itemId <= 0 || (values == null) == (mark == null) ||
                (mark.HasValue && mark.Value is not (Enums.StocktakeItemCheckStatus.Unchecked or
                    Enums.StocktakeItemCheckStatus.Checked or Enums.StocktakeItemCheckStatus.Missing)))
            {
                return Enums.Result.Error;
            }
            try
            {
                using var context = _companyContextFactory.CreateDbContext();
                // Same lock order and transaction boundary as finishing the stocktake.
                await using var transaction = await context.Database.BeginTransactionAsync(
                    System.Data.IsolationLevel.Serializable);
                var stocktake = await context.Stocktakes.SingleOrDefaultAsync(x =>
                    x.Id == stocktakeId && x.RemovedAt == null);
                if (stocktake == null || !stocktake.IsEditable)
                {
                    return Enums.Result.Error;
                }
                var item = await context.StocktakeItems.Include(x => x.Parameters).ThenInclude(x => x.Parameter)
                    .SingleOrDefaultAsync(x => x.Id == itemId && x.StocktakeId == stocktakeId);
                if (item == null)
                {
                    return Enums.Result.Error;
                }
                var oldValues = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ItemId = item.Id, Values = item.GetValues(), item.CheckStatus
                });
                var previousValues = item.SerializeValues();
                var previousMark = item.CheckStatus;
                item.OriginalValuesJson ??= previousValues;
                if (values != null)
                {
                    if (!ValidValues(item, values))
                    {
                        return Enums.Result.Error;
                    }
                    ApplyValues(item, values);
                    item.CheckStatus = item.SerializeValues() == previousValues ? previousMark : item.HasCorrections
                        ? Enums.StocktakeItemCheckStatus.Corrected
                        : Enums.StocktakeItemCheckStatus.Unchecked;
                }
                else
                {
                    item.CheckStatus = mark == Enums.StocktakeItemCheckStatus.Missing
                        ? Enums.StocktakeItemCheckStatus.Missing
                        : item.HasCorrections ? Enums.StocktakeItemCheckStatus.Corrected : mark!.Value;
                }
                stocktake.ChangeStatus(Enums.EntityStatus.InProgress);
                context.EntitiesHistory.Add(new EntityHistory
                {
                    EntityId = stocktakeId, EntityType = Enums.HistoryEntityType.Stocktake,
                    UserId = userId, OccurredAt = DateTime.UtcNow,
                    EventType = MagFlow.Shared.Constants.HistoryEventTypes.ENTITY_UPDATED,
                    OldValuesJson = oldValues,
                    NewValuesJson = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        ItemId = item.Id, Values = item.GetValues(), item.CheckStatus
                    })
                });
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving copied item {ItemId} in stocktake {StocktakeId}.", itemId, stocktakeId);
                return Enums.Result.Error;
            }
        }

        private static void ApplyValues(StocktakeItem item, StocktakeItemValues values)
        {
            item.Quantity = values.Quantity;
            foreach (var parameter in item.Parameters)
            {
                parameter.Value = values.Parameters[parameter.Id];
            }
        }

        private static bool ValidValues(StocktakeItem item, StocktakeItemValues values)
        {
            if (decimal.Round(values.Quantity, 4) != values.Quantity ||
                values.Quantity <= -100000000000000m || values.Quantity >= 100000000000000m ||
                values.Parameters == null || values.Parameters.Count != item.Parameters.Count ||
                item.Parameters.Any(x => !values.Parameters.ContainsKey(x.Id)))
            {
                return false;
            }
            foreach (var parameter in item.Parameters)
            {
                var value = values.Parameters[parameter.Id];
                if (value == null || (parameter.Parameter?.ValueType == Enums.ValueType.Integer &&
                    !long.TryParse(value, out _)) || (parameter.Parameter?.ValueType == Enums.ValueType.Decimal &&
                    !decimal.TryParse(value, out _)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
