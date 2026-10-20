using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope.Interfaces
{
    public interface IWarehouseRepository : IRepository<Warehouse, CompanyDbContext>
    {
        Task<WarehouseLocationsSaveResult> UpdateSector(
            int warehouseId, SectorDTO sector,
            Guid userId, Guid companyId, bool isSuperAdmin);

        Task<WarehouseLocationsSaveResult> UpdateLocations(
            int warehouseId, List<SectorDTO> sectors,
            Guid userId, Guid companyId, bool isSuperAdmin);
    }
}
