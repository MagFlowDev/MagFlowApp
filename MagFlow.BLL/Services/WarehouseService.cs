using Castle.Core.Logging;
using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.DAL.Repositories.CompanyScope;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class WarehouseService : BaseCompanyService<Warehouse, WarehouseDTO>, IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IStocktakeRepository _stocktakeRepository;
        private readonly IItemRepository _itemRepository;

        private readonly INetworkService _networkService;

        public WarehouseService(IWarehouseRepository warehouseRepository,
            IStocktakeRepository stocktakeRepository,
            IItemRepository itemRepository,
            INetworkService networkService, 
            ILogger<WarehouseService> logger) : base(warehouseRepository, networkService, logger)
        {
            _warehouseRepository = warehouseRepository;
            _stocktakeRepository = stocktakeRepository;
            _itemRepository = itemRepository;
            _networkService = networkService;
        }

        public async Task<WarehouseDTO?> GetWarehouse(int id)
        {
            return await base.GetEntityAsync(id, warehouse => warehouse
                .Include(x => x.Items.Where(i => i.SectorId == null))
                .Include(x => x.Sectors.Where(s => s.RemovedAt == null))
                .ThenInclude(y => y.Rows.Where(r => r.RemovedAt == null))
                .ThenInclude(z => z.Slots.Where(s => s.RemovedAt == null)));
        }

        public async Task<Enums.Result> AddWarehouse(WarehouseFormModel model)
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return Enums.Result.Error;
            var entity = model.ToEntity(userId.Value);
            var result = await _warehouseRepository.AddAsync(entity);
            return result;
        }

        public Task<WarehouseLocationsSaveResult> UpdateLocations(int warehouseId, List<SectorDTO> sectors)
            => SaveLocations(warehouseId, sectors, null);

        public async Task<Enums.Result> ChangeWarehouseStatuses(List<int> warehouseIds, Enums.EntityStatus status)
        {
            if (warehouseIds == null || warehouseIds.Count == 0 || warehouseIds.Any(x => x <= 0) ||
                status is not (Enums.EntityStatus.Active or Enums.EntityStatus.Restricted or Enums.EntityStatus.Blocked))
            {
                return Enums.Result.Error;
            }

            var ids = warehouseIds.Distinct().ToList();
            var warehouses = (await _warehouseRepository.GetAllAsync(x => ids.Contains(x.Id))).ToList();
            if (warehouses.Count != ids.Count)
            {
                return Enums.Result.Error;
            }

            foreach (var warehouse in warehouses)
            {
                warehouse.ChangeStatus(status);
            }

            return await _warehouseRepository.UpdateRangeAsync(warehouses);
        }

        public Task<WarehouseLocationsSaveResult> UpdateSector(int warehouseId, SectorDTO sector)
        {
            if (sector == null || sector.Id <= 0)
                return Task.FromResult(WarehouseLocationsSaveResult.InvalidData);

            return SaveLocations(warehouseId, [sector], sector.Id);
        }

        private async Task<WarehouseLocationsSaveResult> SaveLocations(
            int warehouseId, List<SectorDTO> sectors, int? sectorId)
        {
            var principal = _networkService.GetUserPrincipal();
            var userId = _networkService.GetUserId();
            var isSuperAdmin = principal?.IsInRole("SuperAdmin") == true;
            var permission = principal?.Claims.FirstOrDefault(x =>
                string.Equals(x.Type, "perms:Warehouses", StringComparison.OrdinalIgnoreCase))?.Value;
            var edit = (long)Shared.Models.Enumerators.PermissionFlags.Edit;
            if (principal?.Identity?.IsAuthenticated != true || !userId.HasValue ||
                (!isSuperAdmin && (!long.TryParse(permission, out var mask) || (mask & edit) != edit)) ||
                !Guid.TryParse(principal.FindFirst(Shared.Models.Auth.Claims.CompanyClaim)?.Value, out var companyId))
                return WarehouseLocationsSaveResult.Forbidden;

            if (warehouseId <= 0 || sectors == null || sectors.Any(s => s == null ||
                string.IsNullOrWhiteSpace(s.Name) || s.Rows == null || s.Rows.Any(r => r == null ||
                string.IsNullOrWhiteSpace(r.Name) || r.Slots == null || r.Slots.Any(slot =>
                slot == null || string.IsNullOrWhiteSpace(slot.Name)))))
                return WarehouseLocationsSaveResult.InvalidData;

            return sectorId.HasValue
                ? await _warehouseRepository.UpdateSector(warehouseId, sectors[0], userId.Value, companyId, isSuperAdmin)
                : await _warehouseRepository.UpdateLocations(warehouseId, sectors, userId.Value, companyId, isSuperAdmin);
        }

        public async Task<Enums.Result> PrepareStocktake(StocktakeFormModel model)
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
            {
                return Enums.Result.Error;
            }
            return await _stocktakeRepository.PrepareStocktake(model, userId.Value);
        }

        public async Task<QueryResponse<StocktakeDTO>> GetStocktakes(QueryOptions<Stocktake> options)
        {
            var response = await _stocktakeRepository.GetAsync(options, include: options.Includes);
            if (response == null)
            {
                throw new InvalidOperationException("Could not load stocktakes.");
            }
            return new QueryResponse<StocktakeDTO>
            {
                Elements = response.Elements.Select(x => x.ToDTO()).ToList(),
                TotalCount = response.TotalCount
            };
        }
    }
}
