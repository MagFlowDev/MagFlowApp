using MagFlow.EF;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories
{
    public interface IRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
    {
        Task<(TContext? context, IDbContextTransaction? transaction)> BeingTransaction();
        Task<Enums.Result> CommitTransaction(TContext context, IDbContextTransaction transaction);
        Task<Enums.Result> RollbackTransaction(TContext context, IDbContextTransaction transaction);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool tracking = true, bool archive = false);
        Task<QueryResponse<TEntity>?> GetAsync(QueryOptions<TEntity> options, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool archive = false);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool tracking = true, bool archive = false);

        IQueryable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate);

        TEntity? GetById(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
        Task<TEntity?> GetByIdAsync(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool archive = false);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool archive = false);

        Enums.Result Add(TEntity entity, TContext? context = null);
        Task<Enums.Result> AddAsync(TEntity entity, TContext? context = null);

        Enums.Result AddMany(IEnumerable<TEntity> entities, TContext? context = null);
        Task<Enums.Result> AddManyAsync(IEnumerable<TEntity> entities, TContext? context = null);

        Enums.Result Update(TEntity entity, TContext? context = null);
        Task<Enums.Result> UpdateAsync(TEntity entity, TContext? context = null);

        Enums.Result UpdateRange(IEnumerable<TEntity> entities, TContext? context = null);
        Task<Enums.Result> UpdateRangeAsync(IEnumerable<TEntity> entities, TContext? context = null);

        Enums.Result Delete(TEntity entity, TContext? context = null);
        Task<Enums.Result> DeleteAsync(TEntity entity, TContext? context = null);

        Enums.Result DeleteMany(Expression<Func<TEntity, bool>> predicate, TContext? context = null);
        Enums.Result DeleteMany(IEnumerable<TEntity> entities, CompanyDbContext? context = default);
        Task<Enums.Result> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, TContext? context = null);
        Task<Enums.Result> DeleteManyAsync(IEnumerable<TEntity> entities, CompanyDbContext? context = default);

        bool Any(Expression<Func<TEntity, bool>> predicate, bool archive = false);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool archive = false);

        int Count(Expression<Func<TEntity, bool>> predicate, bool archive = false);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool archive = false);

        DbSet<TEntity>? GetSet(bool tracking = true, bool ignoreAutoInclude = false);

        Task<QueryResponse<IEntityHistory>?> GetHistoryAsync(QueryOptions<IEntityHistory> options, Enums.HistoryEntityType entityType, int entityId);
        Task<int> LoadHistoryAsync(TEntity entity, QueryOptions<IEntityHistory>? options = null, CompanyDbContext? companyContext = null, CancellationToken cancellationToken = default);
    }
}
