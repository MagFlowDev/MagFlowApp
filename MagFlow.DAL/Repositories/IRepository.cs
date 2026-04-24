using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
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
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool tracking = true);
        Task<QueryResponse<TEntity>?> GetAsync(QueryOptions<TEntity> options, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool tracking = true);

        IQueryable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate);

        TEntity? GetById(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
        Task<TEntity?> GetByIdAsync(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

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
        Task<Enums.Result> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, TContext? context = null);

        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        DbSet<TEntity>? GetSet(bool tracking = true, bool ignoreAutoInclude = false);
    }
}
