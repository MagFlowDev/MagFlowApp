using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static Grpc.Core.Metadata;

namespace MagFlow.BLL.Services
{
    public class BaseCompanyService<TEntity, TDTO> : IBaseCompanyService<TEntity, TDTO> where TEntity : class, IBaseEntity where TDTO : IBaseDTO
    {
        private readonly IRepository<TEntity, CompanyDbContext> _baseRepository;

        private readonly INetworkService _networkService;

        public BaseCompanyService(IRepository<TEntity, CompanyDbContext> repository,
            INetworkService networkService)
        {
            _baseRepository = repository;
            _networkService = networkService;
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

        public virtual async Task<QueryResponse<EntityHistoryDTO>> GetEntityHistory(int id, Enums.HistoryEntityType entityType, QueryOptions<IEntityHistory> options)
        {
            var queryOptions = options;
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

        public virtual async Task<TDTO?> GetEntityAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var entity = await _baseRepository.GetByIdAsync(id, includes);
            if (entity == null)
                return default;
            var dto = entity.ToDTO<TDTO, TEntity>();
            return dto;
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

        public virtual async Task<Enums.Result> AddEntity(TDTO dto)
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return Enums.Result.Error;
            var entity = dto.ToEntity<TEntity, TDTO>();
            if (typeof(IHistoryEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var historyEntity = (IHistoryEntity)entity;
                var historyEntityType = BLL.Helpers.EnumsHelper.ToHistoryEntityType(typeof(TEntity));
                if (historyEntityType != Enums.HistoryEntityType.Unknown)
                {
                    historyEntity.History.Add(new EntityHistory()
                    {
                        EntityType = historyEntityType,
                        OccurredAt = DateTime.UtcNow,
                        EventType = Shared.Constants.HistoryEventTypes.ENTITY_CREATED,
                        UserId = userId.Value,
                    });
                }
            }
            var result = await _baseRepository.AddAsync(entity);
            return result;
        }

        public virtual async Task<Enums.Result> UpdateEntity(TDTO dto)
        {
            var entity = dto.ToEntity<TEntity, TDTO>();
            if (typeof(IHistoryEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var userId = _networkService.GetUserId();
                var historyEntity = (IHistoryEntity)entity;
                var historyEntityType = BLL.Helpers.EnumsHelper.ToHistoryEntityType(typeof(TEntity));
                if (historyEntityType != Enums.HistoryEntityType.Unknown)
                {
                    historyEntity.History.Add(new EntityHistory()
                    {
                        EntityType = historyEntityType,
                        OccurredAt = DateTime.UtcNow,
                        EventType = Shared.Constants.HistoryEventTypes.ENTITY_UPDATED,
                        UserId = userId
                    });
                }
            }
            var result = await _baseRepository.UpdateAsync(entity);
            return result;
        }



        public async virtual Task<Enums.Result> DeleteEntity(TDTO dto)
        {
            var originalEntity = await _baseRepository.GetByIdAsync(dto.Id);
            if (originalEntity == null)
                return Enums.Result.Error;

            var result = await _baseRepository.DeleteAsync(originalEntity);
            return result;
        }

        public async virtual Task<Enums.Result> DeleteEntities(List<TDTO> dtos)
        {
            var entitiesIds = dtos.Select(x => x.Id).ToList();
            var filterExpression = ContainsIdExpression(entitiesIds);
            var result = await _baseRepository.DeleteManyAsync(filterExpression);
            return result;
        }

        public async virtual Task<Enums.Result> RestoreEntity(TDTO dto)
        {
            var originalEntity = await _baseRepository.GetByIdAsync(dto.Id);
            if (originalEntity == null)
                return Enums.Result.Error;

            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                var removableEntity = (ISoftDeletable)originalEntity;
                removableEntity.RemovedAt = null;
                if (typeof(IStatusEntity).IsAssignableFrom(typeof(TEntity)))
                    ((IStatusEntity)originalEntity).ChangeStatus(Enums.EntityStatus.Available);
                var result = await _baseRepository.UpdateAsync(originalEntity);
            }
            return Enums.Result.Error;
        }

        public async virtual Task<Enums.Result> RestoreEntities(List<TDTO> dtos)
        {
            var entitiesIds = dtos.Select(x => x.Id).ToList();
            if (typeof(TEntity).GetProperties().Any(x => x.Name == "Id") &&
                typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                var filterExpression = ContainsIdExpression(entitiesIds);
                var originalEntities = await _baseRepository.GetAllAsync(filterExpression, archive: true);

                foreach (var originalEntity in originalEntities)
                {
                    var removableEntity = (ISoftDeletable)originalEntity;
                    removableEntity.RemovedAt = null;
                    if (typeof(IStatusEntity).IsAssignableFrom(typeof(TEntity)))
                        ((IStatusEntity)originalEntity).ChangeStatus(Enums.EntityStatus.Available);
                }
                var result = await _baseRepository.UpdateRangeAsync(originalEntities);
                return result;
            }
            return Enums.Result.Error;
        }

        

        private Expression<Func<TEntity, bool>> ContainsIdExpression(IEnumerable<int> entitiesIds)
        {
            var idProperty = typeof(TEntity).GetProperty("Id");

            if(idProperty == null || idProperty.PropertyType != typeof(int))
            {
                return x => false;
            }

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var propertyAccess = Expression.Property(parameter, idProperty);

            var idsConstant = Expression.Constant(entitiesIds);

            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(int));

            var containsExpression = Expression.Call(null, containsMethod, idsConstant, propertyAccess);

            return Expression.Lambda<Func<TEntity, bool>>(containsExpression, parameter);
        }
    }
}
