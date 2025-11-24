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
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);

        IQueryable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate);

        TEntity? GetById(object id);
        Task<TEntity?> GetByIdAsync(object id);

        TEntity? Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Enums.Result Add(TEntity entity);
        Task<Enums.Result> AddAsync(TEntity entity);

        Enums.Result AddMany(IEnumerable<TEntity> entities);
        Task<Enums.Result> AddManyAsync(IEnumerable<TEntity> entities);

        Enums.Result Update(TEntity entity);
        Task<Enums.Result> UpdateAsync(TEntity entity);

        Enums.Result Delete(TEntity entity);
        Task<Enums.Result> DeleteAsync(TEntity entity);

        Enums.Result DeleteMany(Expression<Func<TEntity, bool>> predicate);
        Task<Enums.Result> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        DbSet<TEntity>? GetSet(bool tracking = true, bool ignoreAutoInclude = false);
    }
}
