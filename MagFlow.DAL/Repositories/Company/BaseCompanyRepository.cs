using MagFlow.EF;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MagFlow.DAL.Repositories.Company
{
    public abstract class BaseCompanyRepository<TEntity, TLogger> : IRepository<TEntity> where TEntity : class where TLogger : class
    {
        protected readonly ICoreDbContextFactory _coreContextFactory;
        protected readonly ICompanyDbContextFactory _companyContextFactory;
        protected readonly ILogger<TLogger> _logger;

        public BaseCompanyRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory,
            ILogger<TLogger> logger)
        {
            _coreContextFactory = coreContextFactory;
            _companyContextFactory = companyContextFactory;
            _logger = logger;
        }

        public virtual Enums.Result Add(TEntity entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.Set<TEntity>().Add(entity);
                    context.SaveChanges();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual async Task<Enums.Result> AddAsync(TEntity entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    await context.Set<TEntity>().AddAsync(entity);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual Enums.Result AddMany(IEnumerable<TEntity> entities)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.Set<TEntity>().AddRange(entities);
                    context.SaveChanges();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual async Task<Enums.Result> AddManyAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    await context.Set<TEntity>().AddRangeAsync(entities);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return context.Set<TEntity>().Any(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return await context.Set<TEntity>().AnyAsync(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return context.Set<TEntity>().Count(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return await context.Set<TEntity>().CountAsync(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public virtual Enums.Result Delete(TEntity entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.Set<TEntity>().Remove(entity);
                    context.SaveChanges();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual async Task<Enums.Result> DeleteAsync(TEntity entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.Set<TEntity>().Remove(entity);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual Enums.Result DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var entities = Find(predicate);
                    if (entities == null)
                        return Enums.Result.Error;
                    context.Set<TEntity>().RemoveRange(entities);
                    context.SaveChanges();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual async Task<Enums.Result> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var entities = Find(predicate);
                    if (entities == null)
                        return Enums.Result.Error;
                    context.Set<TEntity>().RemoveRange(entities);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return context.Set<TEntity>().Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }


        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return await context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (predicate != null)
                        query = query.Where(predicate).AsQueryable();
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Array.Empty<TEntity>();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (predicate != null)
                        query = query.Where(predicate).AsQueryable();
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Array.Empty<TEntity>();
            }
        }

        public virtual TEntity? GetById(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();

                    if (include != null)
                    {
                        query = include(query);
                    }

                    var entityType = context.Model.FindEntityType(typeof(TEntity));
                    var keyProperty = entityType?.FindPrimaryKey()?.Properties.Single();

                    if (keyProperty == null)
                        throw new InvalidOperationException("Entity has no primary key.");

                    var parameter = Expression.Parameter(typeof(TEntity), "e");
                    var property = Expression.Property(parameter, keyProperty.Name);
                    var constant = Expression.Constant(id);
                    var equality = Expression.Equal(property, Expression.Convert(constant, property.Type));

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

                    return query.FirstOrDefault(lambda);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();

                    if (include != null)
                    {
                        query = include(query);
                    }

                    var entityType = context.Model.FindEntityType(typeof(TEntity));
                    var keyProperty = entityType?.FindPrimaryKey()?.Properties.Single();

                    if (keyProperty == null)
                        throw new InvalidOperationException("Entity has no primary key.");

                    var parameter = Expression.Parameter(typeof(TEntity), "e");
                    var property = Expression.Property(parameter, keyProperty.Name);
                    var constant = Expression.Constant(id);
                    var equality = Expression.Equal(property, Expression.Convert(constant, property.Type));

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

                    return await query.FirstOrDefaultAsync(lambda);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public virtual Enums.Result Update(TEntity entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.Set<TEntity>().Update(entity);
                    context.SaveChanges();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual async Task<Enums.Result> UpdateAsync(TEntity entity)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    context.Set<TEntity>().Update(entity);
                    await context.SaveChangesAsync();
                }
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enums.Result.Error;
            }
        }

        public virtual IQueryable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    return context.Set<TEntity>().Where(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public virtual DbSet<TEntity>? GetSet(bool tracking = true, bool ignoreAutoInclude = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var entity = context.Set<TEntity>();
                    if (!tracking && ignoreAutoInclude)
                    {
                        entity.IgnoreAutoIncludes().AsNoTracking();
                    }
                    else if (ignoreAutoInclude)
                    {
                        entity.IgnoreAutoIncludes();
                    }
                    else if (!tracking)
                    {
                        entity.AsNoTracking();
                    }
                    return entity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
