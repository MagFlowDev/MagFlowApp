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

        private readonly INetworkService _networkService;

        public WarehouseService(IWarehouseRepository warehouseRepository,
            INetworkService networkService) : base(warehouseRepository, networkService)
        {
            _warehouseRepository = warehouseRepository;
            _networkService = networkService;
        }

        public async Task<WarehouseDTO?> GetWarehouse(int id)
        {
            return await base.GetEntityAsync(id, warehouse => warehouse
                .Include(x => x.Items.Where(i => i.SectorId == null)));
        }

    }
}
