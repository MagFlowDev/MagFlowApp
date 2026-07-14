using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.EF;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class BaseCompanyService<TEntity> : IBaseCompanyService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity, CompanyDbContext> _baseRepository;

        public BaseCompanyService(IRepository<TEntity, CompanyDbContext> repository)
        {
            _baseRepository = repository;
        }

        public virtual async Task<QueryResponse<EntityHistoryDTO>> GetEntityHistory(int id, Enums.HistoryEntityType entityType, int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryOptions = new QueryOptions<IEntityHistory>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                Descending = descending
            };
            var queryResponse = await _baseRepository.GetHistoryAsync(queryOptions, entityType, id);
            return new QueryResponse<EntityHistoryDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<EntityHistoryDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }
    }
}
