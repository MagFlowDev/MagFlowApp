using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Domain.CompanyScope;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class WarehouseRepository : BaseCompanyRepository<Warehouse, WarehouseRepository>, IWarehouseRepository
    {
        public WarehouseRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<WarehouseRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }

        public async Task<WarehouseLocationsSaveResult> UpdateLocations(
            int warehouseId, List<SectorDTO> sectors, Guid userId, Guid companyId, bool isSuperAdmin)
        {
            try
            {
                using var core = _coreContextFactory.CreateDbContext();
                if (!await core.Users.AnyAsync(x => x.Id == userId && x.IsActive && x.RemovedAt == null) ||
                    !await core.Companies.AnyAsync(x => x.Id == companyId && x.IsActive && x.RemovedAt == null) ||
                    (!isSuperAdmin && !await core.CompanyUsers.AnyAsync(x => x.UserId == userId && x.CompanyId == companyId)))
                    return WarehouseLocationsSaveResult.Forbidden;

                using var context = _companyContextFactory.CreateDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                var warehouse = await context.Warehouses
                    .Include(x => x.Sectors).ThenInclude(x => x.Rows).ThenInclude(x => x.Slots)
                    .SingleOrDefaultAsync(x => x.Id == warehouseId && x.RemovedAt == null);
                if (warehouse == null)
                    return WarehouseLocationsSaveResult.InvalidData;

                var sectorIds = new HashSet<int>();
                var rowIds = new HashSet<int>();
                var slotIds = new HashSet<int>();
                foreach (var sector in sectors)
                {
                    var existingSector = warehouse.Sectors.FirstOrDefault(x => x.Id == sector.Id && x.RemovedAt == null);
                    if (sector.Id > 0 && (existingSector == null || !sectorIds.Add(sector.Id)))
                        return WarehouseLocationsSaveResult.InvalidData;
                    foreach (var row in sector.Rows)
                    {
                        var existingRow = existingSector?.Rows.FirstOrDefault(x => x.Id == row.Id && x.RemovedAt == null);
                        if (row.Id > 0 && (existingRow == null || !rowIds.Add(row.Id)))
                            return WarehouseLocationsSaveResult.InvalidData;
                        foreach (var slot in row.Slots)
                            if (slot.Id > 0 && (existingRow?.Slots.Any(x => x.Id == slot.Id && x.RemovedAt == null) != true ||
                                !slotIds.Add(slot.Id)))
                                return WarehouseLocationsSaveResult.InvalidData;
                    }
                }

                var removedSectors = warehouse.Sectors.Where(x => x.RemovedAt == null && !sectorIds.Contains(x.Id)).ToList();
                var removedRows = warehouse.Sectors.SelectMany(x => x.Rows)
                    .Where(x => x.RemovedAt == null && !rowIds.Contains(x.Id)).ToList();
                var removedSlots = warehouse.Sectors.SelectMany(x => x.Rows).SelectMany(x => x.Slots)
                    .Where(x => x.RemovedAt == null && !slotIds.Contains(x.Id)).ToList();
                var removedSectorIds = removedSectors.Select(x => x.Id).ToList();
                var removedRowIds = removedRows.Select(x => x.Id).ToList();
                var removedSlotIds = removedSlots.Select(x => x.Id).ToList();

                if (await context.Items.AnyAsync(x => x.RemovedAt == null &&
                    ((x.SectorId.HasValue && removedSectorIds.Contains(x.SectorId.Value)) ||
                     (x.RowId.HasValue && removedRowIds.Contains(x.RowId.Value)) ||
                     (x.SlotId.HasValue && removedSlotIds.Contains(x.SlotId.Value)))))
                    return WarehouseLocationsSaveResult.LocationInUse;

                var now = DateTime.UtcNow;
                foreach (var sector in removedSectors)
                {
                    sector.RemovedAt = now;
                    sector.RemovedById = userId;
                    sector.ChangeStatus(Enums.EntityStatus.Deleted);
                }
                foreach (var row in removedRows)
                {
                    row.RemovedAt = now;
                    row.RemovedById = userId;
                    row.ChangeStatus(Enums.EntityStatus.Deleted);
                }
                foreach (var slot in removedSlots)
                {
                    slot.RemovedAt = now;
                    slot.RemovedById = userId;
                    slot.ChangeStatus(Enums.EntityStatus.Deleted);
                }

                foreach (var sector in sectors)
                {
                    var entity = sector.Id > 0 ? warehouse.Sectors.Single(x => x.Id == sector.Id) : null;
                    if (entity == null)
                    {
                        entity = new WarehouseSector
                        {
                            CreatedAt = now, CreatedById = userId, Status = Enums.EntityStatus.Active,
                            Rows = new List<WarehouseSectorRow>()
                        };
                        warehouse.Sectors.Add(entity);
                    }
                    entity.Name = sector.Name.Trim();
                    foreach (var row in sector.Rows)
                    {
                        var rowEntity = row.Id > 0 ? entity.Rows.Single(x => x.Id == row.Id) : null;
                        if (rowEntity == null)
                        {
                            rowEntity = new WarehouseSectorRow
                            {
                                CreatedAt = now, CreatedById = userId, Status = Enums.EntityStatus.Active,
                                Slots = new List<WarehouseSectorRowSlot>()
                            };
                            entity.Rows.Add(rowEntity);
                        }
                        rowEntity.Name = row.Name.Trim();
                        foreach (var slot in row.Slots)
                        {
                            var slotEntity = slot.Id > 0 ? rowEntity.Slots.Single(x => x.Id == slot.Id) : null;
                            if (slotEntity == null)
                            {
                                slotEntity = new WarehouseSectorRowSlot
                                {
                                    CreatedAt = now, CreatedById = userId, Status = Enums.EntityStatus.Active
                                };
                                rowEntity.Slots.Add(slotEntity);
                            }
                            slotEntity.Name = slot.Name.Trim();
                        }
                    }
                }

                context.ChangeTracker.DetectChanges();
                var changes = context.ChangeTracker.Entries<StatusEntity>()
                    .Where(x => x.Entity is WarehouseSector or WarehouseSectorRow or WarehouseSectorRowSlot)
                    .Where(x => x.State is EntityState.Added or EntityState.Modified)
                    .Select(x => new
                    {
                        Entry = x,
                        Added = x.State == EntityState.Added,
                        OldValues = x.State == EntityState.Added ? null : JsonSerializer.Serialize(
                            x.OriginalValues.Properties.ToDictionary(p => p.Name, p => x.OriginalValues[p]))
                    }).ToList();

                await context.SaveChangesAsync();
                foreach (var change in changes)
                    context.EntitiesHistory.Add(new EntityHistory
                    {
                        EntityId = change.Entry.Entity.Id,
                        EntityType = change.Entry.Entity.EntityType,
                        UserId = userId,
                        OccurredAt = now,
                        EventType = change.Added ? Shared.Constants.HistoryEventTypes.ENTITY_CREATED : Shared.Constants.HistoryEventTypes.ENTITY_UPDATED,
                        OldValuesJson = change.OldValues,
                        NewValuesJson = JsonSerializer.Serialize(change.Entry.CurrentValues.Properties
                            .ToDictionary(p => p.Name, p => change.Entry.CurrentValues[p]))
                    });
                if (changes.Count > 0)
                {
                    context.EntitiesHistory.Add(new EntityHistory
                    {
                        EntityId = warehouse.Id, EntityType = warehouse.EntityType, UserId = userId,
                        OccurredAt = now, EventType = Shared.Constants.HistoryEventTypes.ENTITY_UPDATED
                    });
                    await context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return WarehouseLocationsSaveResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving locations for warehouse {WarehouseId}.", warehouseId);
                return WarehouseLocationsSaveResult.Error;
            }
        }
    }
}
