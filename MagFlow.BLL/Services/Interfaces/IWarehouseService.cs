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
        Task<StocktakeDetailsDTO?> GetStocktakeDetails(int stocktakeId);
        Task<Enums.Result> FinishStocktake(int stocktakeId);
        Task<Enums.Result> ResumeStocktake(int stocktakeId);
        Task<Enums.Result> ApproveStocktake(int stocktakeId);
        Task<Enums.Result> DeleteStocktake(int stocktakeId);
        Task<Enums.Result> CancelStocktake(int stocktakeId);
        Task<Enums.Result> SaveStocktakeItem(int stocktakeId, int itemId,
            StocktakeItemValues? values, Enums.StocktakeItemCheckStatus? mark);
        Task<Enums.Result> PrepareStocktake(StocktakeFormModel model);
        Task<QueryResponse<StocktakeDTO>> GetStocktakes(QueryOptions<Stocktake> options);
        Task<Enums.Result> ChangeWarehouseStatuses(List<int> warehouseIds, Enums.EntityStatus status);
        Task<WarehouseLocationsSaveResult> UpdateLocations(int warehouseId, List<SectorDTO> sectors);
        Task<WarehouseLocationsSaveResult> UpdateSector(int warehouseId, SectorDTO sector);
    }
}
