using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IBaseCompanyService<TEntity, TDTO> where TEntity : IBaseEntity where TDTO : IBaseDTO
    {
        Task<QueryResponse<EntityHistoryDTO>> GetEntityHistory(int id, Enums.HistoryEntityType entityType, int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<EntityHistoryDTO>> GetEntityHistory(int id, Enums.HistoryEntityType entityType, QueryOptions<IEntityHistory> options);
        Task<QueryResponse<TDTO>> GetManyAsync(QueryOptions<TEntity> options);
        Task<TDTO?> GetEntityAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);
        
        Task<Enums.Result> AddEntity(TDTO dto);
        Task<Enums.Result> UpdateEntity(TDTO dto);
        Task<Enums.Result> DeleteEntity(TDTO dto);
        Task<Enums.Result> DeleteEntities(List<TDTO> dtos);
        Task<Enums.Result> RestoreEntity(TDTO dto);
        Task<Enums.Result> RestoreEntities(List<TDTO> dtos);
    }
}
