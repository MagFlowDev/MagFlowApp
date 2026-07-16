using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CompanyScope;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class ItemService : BaseCompanyService<Item>, IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly INetworkService _networkService;

        private readonly ILogger<ItemService> _logger;

        public ItemService(IItemRepository itemRepository, 
            INetworkService networkService,
            ILogger<ItemService> logger) : base(itemRepository)
        {
            _itemRepository = itemRepository;
            _networkService = networkService;
            _logger = logger;
        }

        public async Task<ItemDTO?> GetItem(int id)
        {
            var product = await _itemRepository.GetByIdAsync(id, item => item
                .Include(x => x.Product).ThenInclude(y => y.Components).ThenInclude(z => z.Component)
                .Include(x => x.DefaultUnit)
                .Include(x => x.CreatedBy)
                .Include(x => x.Parameters).ThenInclude(y => y.Parameter).ThenInclude(z => z.Unit));
            var dto = product?.ToDTO();
            return dto;
        }

        public async Task<QueryResponse<ItemDTO>> GetItems(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _itemRepository.GetAsync(new QueryOptions<Item>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                Descending = descending
            }, items => items
                .Include(x => x.Product).ThenInclude(y => y.Type).ThenInclude(z => z.Category)
                .Include(x => x.Product).ThenInclude(y => y.Category)
                .Include(x => x.Product).ThenInclude(y => y.Unit)
                .Include(x => x.Product).ThenInclude(y => y.Components).ThenInclude(z => z.Component)
                .Include(x => x.DefaultUnit)
                .Include(x => x.CreatedBy)
                .Include(x => x.Parameters).ThenInclude(y => y.Parameter).ThenInclude(z => z.Unit)
                .Include(x => x.Components));
            return new QueryResponse<ItemDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ItemDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        public async Task<QueryResponse<ItemDTO>> GetItems(QueryOptions<Item> options)
        {
            var queryResponse = await _itemRepository.GetAsync(options, items => items
                .Include(x => x.Product).ThenInclude(y => y.Type).ThenInclude(z => z.Category)
                .Include(x => x.Product).ThenInclude(y => y.Category)
                .Include(x => x.Product).ThenInclude(y => y.Unit)
                .Include(x => x.Product).ThenInclude(y => y.Components).ThenInclude(z => z.Component)
                .Include(x => x.DefaultUnit)
                .Include(x => x.CreatedBy)
                .Include(x => x.Parameters).ThenInclude(y => y.Parameter).ThenInclude(z => z.Unit)
                .Include(x => x.Components));
            return new QueryResponse<ItemDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ItemDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        public async Task<QueryResponse<ItemDTO>> GetArchive(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _itemRepository.GetAsync(new QueryOptions<Item>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                Descending = descending
            }, include: items => items
                .Include(x => x.Product).ThenInclude(y => y.Type).ThenInclude(z => z.Category)
                .Include(x => x.Product).ThenInclude(y => y.Category)
                .Include(x => x.Product).ThenInclude(y => y.Unit)
                .Include(x => x.DefaultUnit)
                .Include(x => x.CreatedBy)
                .Include(x => x.Parameters).ThenInclude(y => y.Parameter).ThenInclude(z => z.Unit),  archive: true);
            return new QueryResponse<ItemDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ItemDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }





        public async Task<Enums.Result> AddItem(ItemFormModel model)
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return Enums.Result.Error;
            var unit = model.ToEntity(userId.Value);
            var contextTransaction = await _itemRepository.BeingTransaction();
            var result = Enums.Result.Error;
            if (contextTransaction.context == null || contextTransaction.transaction == null)
                return result;

            try
            {
                result = await _itemRepository.AddAsync(unit);
                if(result != Enums.Result.Success)
                {
                    await _itemRepository.RollbackTransaction(contextTransaction.context, contextTransaction.transaction);
                    return result;
                }

                var itemQuantity = model.Components.Components
                        .SelectMany(x => x.Components)
                        .Where(x => x.Item.TempQuantity.HasValue)
                        .Select(x => new { Id = x.Item.Id, NewQuantity = x.Item.TempQuantity!.Value })
                        .ToDictionary(x => x.Id, x => x.NewQuantity);

                result = await _itemRepository.UpdateItemQuantity(itemQuantity, Enums.ItemStatus.Used, contextTransaction.context);
                if(result != Enums.Result.Success)
                {
                    await _itemRepository.RollbackTransaction(contextTransaction.context, contextTransaction.transaction);
                    return result;
                }

                result = await _itemRepository.CommitTransaction(contextTransaction.context, contextTransaction.transaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _itemRepository.RollbackTransaction(contextTransaction.context, contextTransaction.transaction);
                return Enums.Result.Error;
            }
        }





        public async Task<Enums.Result> BlockItem(ItemDTO itemDTO, bool unblock = false)
        {
            if (itemDTO == null || itemDTO.Id == 0)
                return Enums.Result.Error;
            var item = await _itemRepository.GetByIdAsync(itemDTO.Id);
            if (item == null)
                return Enums.Result.Error;

            item.Status = unblock ? (item.Status == Enums.ItemStatus.Blocked ? Enums.ItemStatus.Available : item.Status) : Enums.ItemStatus.Blocked;
            var result = await _itemRepository.UpdateAsync(item);
            return result;
        }

        public async Task<Enums.Result> BlockItems(List<ItemDTO> itemsDTO, bool unblock = false)
        {
            if (!itemsDTO?.Any() == true)
                return Enums.Result.Error;

            var itemsIds = itemsDTO
                .Where(x => x != null && x.Id != 0)
                .Select(x => x.Id)
                .ToList();
            var items = await _itemRepository.GetAllAsync(x => itemsIds.Contains(x.Id));
            if (items == null)
                return Enums.Result.Error;

            foreach (var item in items)
            {
                item.Status = unblock ? (item.Status == Enums.ItemStatus.Blocked ? Enums.ItemStatus.Available : item.Status) : Enums.ItemStatus.Blocked;
            }
            var result = await _itemRepository.UpdateRangeAsync(items);
            return result;
        }

        public async Task<Enums.Result> DeleteItem(ItemDTO itemDTO)
        {
            var originalItem = await _itemRepository.GetByIdAsync(itemDTO.Id);
            if (originalItem == null)
                return Enums.Result.Error;

            var result = await _itemRepository.DeleteAsync(originalItem);
            return result;
        }

        public async Task<Enums.Result> DeleteItems(List<ItemDTO> itemsDTOs)
        {
            var itemsIds = itemsDTOs.Select(x => x.Id).ToList();
            var result = await _itemRepository.DeleteManyAsync(x => itemsIds.Contains(x.Id));
            return result;
        }

        public async Task<Enums.Result> RestoreItem(ItemDTO itemDTO)
        {
            var originalItem = await _itemRepository.GetByIdAsync(itemDTO.Id);
            if (originalItem == null)
                return Enums.Result.Error;

            originalItem.RemovedAt = null;
            originalItem.Status = Enums.ItemStatus.Available;
            var result = await _itemRepository.UpdateAsync(originalItem);
            return result;
        }

        public async Task<Enums.Result> RestoreItems(List<ItemDTO> itemsDTOs)
        {
            var itemsIds = itemsDTOs.Select(x => x.Id).ToList();
            var originalItems = await _itemRepository.GetAllAsync(x => itemsIds.Contains(x.Id), archive: true);

            foreach(var originalItem in originalItems)
            {
                originalItem.RemovedAt = null;
                originalItem.Status = Enums.ItemStatus.Available;
            }
            var result = await _itemRepository.UpdateRangeAsync(originalItems);
            return result;
        }
    }
}
