using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Constants;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Domain.CompanyScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public class ContractorRepository : BaseCompanyRepository<Contractor, ContractorRepository>, IContractorRepository
    {
        public ContractorRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<ContractorRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }

        public async Task<Enums.Result> ChangeStatuses(IReadOnlyCollection<int> ids, Enums.EntityStatus status, Guid userId)
        {
            try
            {
                await using var context = _companyContextFactory.CreateDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

                var contractors = await context.Contractors
                    .AsTracking()
                    .Where(contractor => ids.Contains(contractor.Id) && contractor.RemovedAt == null)
                    .ToListAsync();

                if (contractors.Count != ids.Count ||
                    contractors.Any(contractor => contractor.Status == Enums.EntityStatus.Deleted))
                    return Enums.Result.Error;

                var occurredAt = DateTime.UtcNow;
                foreach (var contractor in contractors.Where(contractor => contractor.Status != status))
                {
                    var previousStatus = contractor.Status;
                    contractor.ChangeStatus(status);

                    context.EntitiesHistory.Add(new EntityHistory
                    {
                        EntityId = contractor.Id,
                        EntityType = contractor.EntityType,
                        EventType = HistoryEventTypes.ENTITY_UPDATED,
                        OccurredAt = occurredAt,
                        UserId = userId,
                        OldValuesJson = JsonSerializer.Serialize(new { Status = previousStatus }),
                        NewValuesJson = JsonSerializer.Serialize(new { Status = status })
                    });
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Enums.Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to change contractor statuses to {Status}", status);
                return Enums.Result.Error;
            }
        }

        public async Task<Enums.Result> RestoreArchivedContractors(IReadOnlyCollection<int> ids)
        {
            try
            {
                await using var context = _companyContextFactory.CreateDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

                var contractors = await context.Contractors
                    .AsTracking()
                    .Where(contractor => ids.Contains(contractor.Id) && contractor.RemovedAt != null)
                    .ToListAsync();

                if (contractors.Count != ids.Count)
                    return Enums.Result.Error;

                foreach (var contractor in contractors)
                {
                    contractor.RemovedAt = null;
                    contractor.ChangeStatus(Enums.EntityStatus.Active);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Enums.Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to restore archived contractors");
                return Enums.Result.Error;
            }
        }
    }
}
