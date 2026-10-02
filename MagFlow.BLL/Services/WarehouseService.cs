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
                .Include(x => x.Sectors).ThenInclude(y => y.Rows).ThenInclude(z => z.Slots));
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

        public async Task<Enums.Result> CreateStocktake(StocktakeFormModel model)
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return Enums.Result.Error;
            try
            {
                var stocktake = model.Warehouse.CreateStocktake(model.Type, model.PlannedDate, userId.Value);
                var result = await _stocktakeRepository.AddAsync(stocktake);
                if (result != Enums.Result.Success)
                    return result;

                var warehouseItems = await _itemRepository.GetAllAsync(x =>
                    x.WarehouseId == model.Warehouse.Id &&
                    x.SectorId == null &&
                    x.RowId == null &&
                    x.SlotId == null);
                
                if(warehouseItems.Any())
                {
                    var stocktakeItems = warehouseItems.CreateStocktakeItems(stocktake.Id);
                    // Add items to stocktake
                }

                foreach(var sector in model.SelectedSectors)
                {
                    var sectorItems = await _itemRepository.GetAllAsync(x =>
                        x.WarehouseId == model.Warehouse.Id &&
                        x.SectorId == sector.Id);
                    if(sectorItems.Any())
                    {
                        var stocktakeItems = sectorItems.CreateStocktakeItems(stocktake.Id);
                        // Add items to stocktake
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while creating stocktake.");
            }
            return Enums.Result.Error;
        }
    }
}
