using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.DAL.Repositories.CompanyScope;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class WarehouseService : BaseCompanyService<Warehouse, WarehouseDTO>, IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository) : base(warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<WarehouseDTO?> GetWarehouse(int id)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(id, warehouse => warehouse
                .Include(x => x.Items)
                .Include(x => x.Storages));
            var dto = warehouse?.ToDTO();
            return dto;
        }

        public async Task<QueryResponse<WarehouseDTO>> GetWarehouses(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _warehouseRepository.GetAsync(new QueryOptions<Warehouse>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new Expression<Func<Warehouse, string?>>[]
                {
                    u => u.Name
                },
                SortBy = sortBy,
                Descending = descending
            }, warehouses => warehouses
                .Include(x => x.Items)
                .Include(x => x.Storages));
            return new QueryResponse<WarehouseDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<WarehouseDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

    }
}
