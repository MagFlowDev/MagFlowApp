using MagFlow.DAL.Helpers;
using MagFlow.EF;
using MagFlow.EF.MultiTenancy;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MagFlow.DAL.Repositories.CompanyScope
{
    public abstract class BaseCompanyRepository<TEntity, TLogger> : IRepository<TEntity, CompanyDbContext> where TEntity : class where TLogger : class
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

        public virtual async Task<(CompanyDbContext context, IDbContextTransaction transaction)> BeingTransaction()
        {
            CompanyDbContext? context = null;
            IDbContextTransaction? transaction = null;
            try
            {
                context = _companyContextFactory.CreateDbContext();
                transaction = await context.Database.BeginTransactionAsync();
                return (context, transaction);
            }
            catch(Exception)
            {
                if (transaction != null)
                    await transaction.RollbackAsync();

                transaction?.Dispose();
                context?.Dispose();
                return (null, null);
            }
        }

        public virtual async Task<Enums.Result> CommitTransaction(CompanyDbContext context, IDbContextTransaction transaction)
        {
            try
            {
                await transaction.CommitAsync();
                transaction.Dispose();
                context.Dispose();
                return Enums.Result.Success;
            }
            catch(Exception ex)
            {
                transaction?.Dispose();
                context?.Dispose();
                return Enums.Result.Error;
            }
        }

        public virtual async Task<Enums.Result> RollbackTransaction(CompanyDbContext context, IDbContextTransaction transaction)
        {
            try
            {
                await transaction.CommitAsync();
                transaction.Dispose();
                context.Dispose();
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                transaction?.Dispose();
                context?.Dispose();
                return Enums.Result.Error;
            }
        }


        public virtual Enums.Result Add(TEntity entity, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        context.Set<TEntity>().Add(entity);
                        context.SaveChanges();
                    }
                }
                else
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

        public virtual async Task<Enums.Result> AddAsync(TEntity entity, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        await context.Set<TEntity>().AddAsync(entity);
                        await context.SaveChangesAsync();
                    }
                }
                else
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

        public virtual Enums.Result AddMany(IEnumerable<TEntity> entities, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        context.Set<TEntity>().AddRange(entities);
                        context.SaveChanges();
                    }
                }
                else
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

        public virtual async Task<Enums.Result> AddManyAsync(IEnumerable<TEntity> entities, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        await context.Set<TEntity>().AddRangeAsync(entities);
                        await context.SaveChangesAsync();
                    }
                }
                else
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

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    return query.Any(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    return await query.AnyAsync(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    return query.Count(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    return await query.CountAsync(predicate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public virtual Enums.Result Delete(TEntity entity, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                        {
                            ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                            var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                            if (isActiveProperty != null)
                                isActiveProperty.SetValue(entity, false);
                            context.Set<TEntity>().Update(entity);
                        }
                        else
                        {
                            context.Set<TEntity>().Remove(entity);
                        }
                        context.SaveChanges();
                    }
                }
                else
                {
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                        var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                        if (isActiveProperty != null)
                            isActiveProperty.SetValue(entity, false);
                        context.Set<TEntity>().Update(entity);
                    }
                    else
                    {
                        context.Set<TEntity>().Remove(entity);
                    }
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

        public virtual async Task<Enums.Result> DeleteAsync(TEntity entity, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                        {
                            ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                            var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                            if(isActiveProperty != null)
                                isActiveProperty.SetValue(entity, false);
                            context.Set<TEntity>().Update(entity);
                        }
                        else
                        {
                            context.Set<TEntity>().Remove(entity);
                        }
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                        var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                        if (isActiveProperty != null)
                            isActiveProperty.SetValue(entity, false);
                        context.Set<TEntity>().Update(entity);
                    }
                    else
                    {
                        context.Set<TEntity>().Remove(entity);
                    }
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

        public virtual Enums.Result DeleteMany(Expression<Func<TEntity, bool>> predicate, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        var entities = Find(predicate, context);
                        if (entities == null)
                            return Enums.Result.Error;
                        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                        {
                            foreach (var entity in entities)
                            {
                                ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                                var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                                if (isActiveProperty != null)
                                    isActiveProperty.SetValue(entity, false);
                            }
                            context.Set<TEntity>().UpdateRange(entities);
                        }
                        else
                        {
                            context.Set<TEntity>().RemoveRange(entities);
                        }
                        context.SaveChanges();
                    }
                }
                else
                {
                    var entities = Find(predicate, context);
                    if (entities == null)
                        return Enums.Result.Error;
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        foreach (var entity in entities)
                        {
                            ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                            var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                            if (isActiveProperty != null)
                                isActiveProperty.SetValue(entity, false);
                        }
                        context.Set<TEntity>().UpdateRange(entities);
                    }
                    else
                    {
                        context.Set<TEntity>().RemoveRange(entities);
                    }
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

        public virtual Enums.Result DeleteMany(IEnumerable<TEntity> entities, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        foreach (var entity in entities)
                        {
                            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                            {
                                ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                                var isActiveProperty = entity.GetType()
                                    .GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                                if (isActiveProperty != null)
                                    isActiveProperty.SetValue(entity, false);
                                context.Set<TEntity>().Update(entity);
                            }
                            else
                            {
                                context.Set<TEntity>().Remove(entity);
                            }
                        }
                        context.SaveChanges();
                    }
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                        {
                            ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                            var isActiveProperty = entity.GetType()
                                .GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                            if (isActiveProperty != null)
                                isActiveProperty.SetValue(entity, false);
                            context.Set<TEntity>().Update(entity);
                        }
                        else
                        {
                            context.Set<TEntity>().Remove(entity);
                        }
                    }
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

        public virtual async Task<Enums.Result> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        var entities = Find(predicate, context);
                        if (entities == null)
                            return Enums.Result.Error;
                        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                        {
                            foreach (var entity in entities)
                            {
                                ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                                var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                                if (isActiveProperty != null)
                                    isActiveProperty.SetValue(entity, false);
                            }
                            context.Set<TEntity>().UpdateRange(entities);
                        }
                        else
                        {
                            context.Set<TEntity>().RemoveRange(entities);
                        }
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var entities = Find(predicate, context);
                    if (entities == null)
                        return Enums.Result.Error;
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        foreach (var entity in entities)
                        {
                            ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                            var isActiveProperty = entity.GetType().GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                            if (isActiveProperty != null)
                                isActiveProperty.SetValue(entity, false);
                        }
                        context.Set<TEntity>().UpdateRange(entities);
                    }
                    else
                    {
                        context.Set<TEntity>().RemoveRange(entities);
                    }
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

        public virtual async Task<Enums.Result> DeleteManyAsync(IEnumerable<TEntity> entities, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        foreach (var entity in entities)
                        {
                            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                            {
                                ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                                var isActiveProperty = entity.GetType()
                                    .GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                                if (isActiveProperty != null)
                                    isActiveProperty.SetValue(entity, false);
                                context.Set<TEntity>().Update(entity);
                            }
                            else
                            {
                                context.Set<TEntity>().Remove(entity);
                            }
                        }
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                        {
                            ((ISoftDeletable)entity).RemovedAt = DateTime.UtcNow;
                            var isActiveProperty = entity.GetType()
                                .GetProperty(MagFlow.Shared.Constants.DatabaseConstants.ISACTIVE_PROPERTY);
                            if (isActiveProperty != null)
                                isActiveProperty.SetValue(entity, false);
                            context.Set<TEntity>().Update(entity);
                        }
                        else
                        {
                            context.Set<TEntity>().Remove(entity);
                        }
                    }
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

        public virtual TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    if (include != null)
                        query = include(query);
                    return query.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }


        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    if (include != null)
                        query = include(query);
                    return await query.Where(predicate).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool tracking = true, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    if (include != null)
                        query = include(query);
                    if (predicate != null)
                        query = query.Where(predicate).AsQueryable();
                    if (!tracking)
                        return query.AsNoTracking().ToList();
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Array.Empty<TEntity>();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool tracking = true, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    if (include != null)
                        query = include(query);
                    if (predicate != null)
                        query = query.Where(predicate).AsQueryable();
                    if (!tracking)
                        return await query.AsNoTracking().ToListAsync();
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Array.Empty<TEntity>();
            }
        }

        public virtual async Task<QueryResponse<TEntity>?> GetAsync(QueryOptions<TEntity> options, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool archive = false)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.Set<TEntity>().AsQueryable();
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
                    {
                        if (archive)
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt != null);
                        else
                            query = query.Where(e => ((ISoftDeletable)e).RemovedAt == null);
                    }
                    if (include != null)
                        query = include(query);
                    query = query.ApplyColumnFilters(options.Filters);
                    query = query.ExcludeColumnFilters(options.Exludes);
                    query = query.ApplyMultiColumnSearch(options.Search, options.SearchColumns);
                    query = query.SortBy(options.SortBy, options.Descending);
                    var count = await query.CountAsync();
                    var entities = await query.Paginate(options.PageNumber, options.PageSize).ToListAsync();
                    return new QueryResponse<TEntity>()
                    {
                        Elements = entities,
                        TotalCount = count
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
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

        public virtual Enums.Result Update(TEntity entity, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        if (context.Set<TEntity>().Contains(entity))
                            context.Set<TEntity>().Update(entity);
                        else
                            return Add(entity);
                        context.SaveChanges();
                    }
                }
                else
                {
                    if (context.Set<TEntity>().Contains(entity))
                        context.Set<TEntity>().Update(entity);
                    else
                        return Add(entity);
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

        public virtual async Task<Enums.Result> UpdateAsync(TEntity entity, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        if (await context.Set<TEntity>().ContainsAsync(entity))
                            context.Set<TEntity>().Update(entity);
                        else
                            return await AddAsync(entity);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (await context.Set<TEntity>().ContainsAsync(entity))
                        context.Set<TEntity>().Update(entity);
                    else
                        return await AddAsync(entity);
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

        public virtual Enums.Result UpdateRange(IEnumerable<TEntity> entities, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            foreach (var entity in entities)
                            {
                                if (context.Set<TEntity>().Contains(entity))
                                    context.Set<TEntity>().Update(entity);
                                else
                                {
                                    var temp = Add(entity);
                                    if (temp != Enums.Result.Success)
                                    {
                                        transaction.Rollback();
                                        return temp;
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        context.SaveChanges();
                    }
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        if (context.Set<TEntity>().Contains(entity))
                            context.Set<TEntity>().Update(entity);
                        else
                            Add(entity);
                    }
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

        public virtual async Task<Enums.Result> UpdateRangeAsync(IEnumerable<TEntity> entities, CompanyDbContext? context = default)
        {
            try
            {
                if (context == null)
                {
                    using (context = _companyContextFactory.CreateDbContext())
                    {
                        using (var transaction = await context.Database.BeginTransactionAsync())
                        {
                            foreach (var entity in entities)
                            {
                                if (await context.Set<TEntity>().ContainsAsync(entity))
                                    context.Set<TEntity>().Update(entity);
                                else
                                {
                                    var temp = await AddAsync(entity);
                                    if (temp != Enums.Result.Success)
                                    {
                                        await transaction.RollbackAsync();
                                        return temp;
                                    }
                                }
                            }
                            await transaction.CommitAsync();
                        }
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        if (await context.Set<TEntity>().ContainsAsync(entity))
                            context.Set<TEntity>().Update(entity);
                        else
                            await AddAsync(entity);
                    }
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

        public virtual IQueryable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate, CompanyDbContext? companyContext = null)
        {
            try
            {
                if (companyContext != null)
                {
                    return companyContext.Set<TEntity>().Where(predicate);
                }

                else
                {
                    using (var context = _companyContextFactory.CreateDbContext())
                    {
                        return context.Set<TEntity>().Where(predicate);
                    }
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

        public async Task<int> LoadHistoryAsync(TEntity entity, QueryOptions<IEntityHistory>? options = null, CompanyDbContext? companyContext = null, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!typeof(IHistoryEntity).IsAssignableFrom(typeof(TEntity)))
                    return -1;

                var historyEntity = (IHistoryEntity)entity;
                if (historyEntity == null)
                    return -1;

                if (companyContext != null)
                {
                    if (options == null)
                    {
                        var history = companyContext.EntitiesHistory
                            .Include(x => x.User)
                            .Where(x =>
                                x.EntityType == historyEntity.EntityType &&
                                x.EntityId == historyEntity.Id)
                            .OrderByDescending(x => x.OccurredAt);
                        historyEntity.History = await history.Cast<IEntityHistory>().ToListAsync();
                        return await history.CountAsync();
                    }
                    else
                    {
                        var query = companyContext.EntitiesHistory
                            .Include(x => x.User)
                            .Where(x =>
                                x.EntityType == historyEntity.EntityType &&
                                x.EntityId == historyEntity.Id)
                            .OrderByDescending(x => x.OccurredAt)
                            .AsQueryable();
                        query = query.ApplyColumnFilters(options.Filters);
                        query = query.ExcludeColumnFilters(options.Exludes);
                        query = (IQueryable<Domain.CompanyScope.EntityHistory>)query.ApplyMultiColumnSearch(options.Search, options.SearchColumns);
                        query = query.SortBy(options.SortBy, options.Descending);
                        var count = await query.CountAsync();
                        var entities = await query.Paginate(options.PageNumber, options.PageSize).ToListAsync();
                        historyEntity.History = entities.Cast<IEntityHistory>().ToList();
                        return count;
                    }
                }

                else
                {
                    using (var context = _companyContextFactory.CreateDbContext())
                    {
                        if(options == null)
                        {
                            var history = context.EntitiesHistory
                                .Include(x => x.User)
                                .Where(x =>
                                    x.EntityType == historyEntity.EntityType &&
                                    x.EntityId == historyEntity.Id)
                                .OrderByDescending(x => x.OccurredAt);
                            historyEntity.History = await history.Cast<IEntityHistory>().ToListAsync();
                            return await history.CountAsync();
                        }
                        else
                        {
                            var query = context.EntitiesHistory
                                .Include(x => x.User)
                                .Where(x =>
                                    x.EntityType == historyEntity.EntityType &&
                                    x.EntityId == historyEntity.Id)
                                .OrderByDescending(x => x.OccurredAt)
                                .AsQueryable();
                            query = query.ApplyColumnFilters(options.Filters);
                            query = query.ExcludeColumnFilters(options.Exludes);
                            query = (IQueryable<Domain.CompanyScope.EntityHistory>)query.ApplyMultiColumnSearch(options.Search, options.SearchColumns);
                            query = query.SortBy(options.SortBy, options.Descending);
                            var count = await query.CountAsync();
                            var entities = await query.Paginate(options.PageNumber, options.PageSize).ToListAsync();
                            historyEntity.History = entities.Cast<IEntityHistory>().ToList();
                            return count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public async Task<QueryResponse<IEntityHistory>?> GetHistoryAsync(QueryOptions<IEntityHistory> options, Enums.HistoryEntityType entityType, int entityId)
        {
            try
            {
                using (var context = _companyContextFactory.CreateDbContext())
                {
                    var query = context.EntitiesHistory
                                .Include(x => x.User)
                                .Where(x =>
                                    x.EntityType == entityType &&
                                    x.EntityId == entityId)
                                .OrderByDescending(x => x.OccurredAt)
                                .AsQueryable();
                    query = query.ApplyColumnFilters(options.Filters);
                    query = query.ExcludeColumnFilters(options.Exludes);
                    query = (IQueryable<Domain.CompanyScope.EntityHistory>)query.ApplyMultiColumnSearch(options.Search, options.SearchColumns);
                    query = query.SortBy(options.SortBy, options.Descending);
                    var count = await query.CountAsync();
                    var entities = await query.Paginate(options.PageNumber, options.PageSize).Cast<IEntityHistory>().ToListAsync();
                    return new QueryResponse<IEntityHistory>()
                    {
                        Elements = entities,
                        TotalCount = count
                    };
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
