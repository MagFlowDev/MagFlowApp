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
                await transaction.CommitAsync();
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing stocktake for warehouse {WarehouseId}.", warehouseId);
                return Enums.Result.Error;
            }
        }
    }
}
