using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.EF;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class BaseCompanyService<TEntity, TDTO> : IBaseCompanyService<TEntity, TDTO> where TEntity : class where TDTO : BaseDTO
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

        public virtual async Task<QueryResponse<TDTO>> GetManyAsync(QueryOptions<TEntity> options)
        {
            var queryResponse = await _baseRepository.GetAsync(options, include: options.Includes);
            return new QueryResponse<TDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO<TDTO, TEntity>();
                    return dto;
                }).ToList() ?? new List<TDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }
    }
}
