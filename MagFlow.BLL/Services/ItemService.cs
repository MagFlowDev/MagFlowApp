using MagFlow.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.BLL.Mappers.Domain.CompanyScope;

namespace MagFlow.BLL.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
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
            });
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
            }, archive: true);
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
    }
}
