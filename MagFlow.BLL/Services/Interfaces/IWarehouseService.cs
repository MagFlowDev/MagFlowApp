using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IWarehouseService : IBaseCompanyService<Warehouse, WarehouseDTO>
    {
        Task<WarehouseDTO?> GetWarehouse(int id);
        Task<Enums.Result> AddWarehouse(WarehouseFormModel model);
    }
}
