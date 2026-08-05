using MagFlow.Shared.Models;
using System.Linq.Expressions;

namespace MagFlow.DAL.Helpers
{
    public static class QueryHelper
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 0)
                pageNumber = 0;

            if (pageSize < 1)
                pageSize = 25;
            
            return query
                .Skip(pageNumber * pageSize)
                .Take(pageSize);
        }
        
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string? sortBy, bool descending = false)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = descending ? "OrderByDescending" : "OrderBy";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(resultExpression);
        }
        
        public static IQueryable<T> ApplyColumnFilters<T>(this IQueryable<T> query, List<ColumnFilter>? filters)
        {
            if (filters == null || !filters.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "e");
            Expression? combinedExpression = null;

            foreach(var filter in filters)
            {
                if (string.IsNullOrEmpty(filter.PropertyName))
                    continue;

                Expression propertyAccess = parameter;
                foreach(var member in filter.PropertyName.Split('.'))
                {
                    propertyAccess = Expression.PropertyOrField(propertyAccess, member);
                }

                var targetType = propertyAccess.Type;
                var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                Expression filterExpression;

                if(filter.Operator == FilterOperator.IsEmpty)
                {
                    var nullConstant = Expression.Constant(null, targetType);
                    filterExpression = Expression.Equal(propertyAccess, nullConstant);

                    if (targetType == typeof(string))
                    {
                        var emptyConstant = Expression.Constant(string.Empty, typeof(string));
                        var isEmptyString = Expression.Equal(propertyAccess, emptyConstant);
                        filterExpression = Expression.OrElse(filterExpression, isEmptyString);
                    }
                }
                else if(filter.Operator == FilterOperator.IsNotEmpty)
                {
                    var nullConstant = Expression.Constant(null, targetType);
                    filterExpression = Expression.NotEqual(propertyAccess, nullConstant);

                    if (targetType == typeof(string))
                    {
                        var emptyConstant = Expression.Constant(string.Empty, typeof(string));
                        var isNotEmptyString = Expression.NotEqual(propertyAccess, emptyConstant);
                        filterExpression = Expression.AndAlso(filterExpression, isNotEmptyString);
                    }
                }
                else
                {
                    if (filter.Value == null || string.IsNullOrWhiteSpace(filter.Value.ToString()))
                        continue;

                    object? convertedValue;
                    try
                    {
                        if (underlyingType.IsEnum)
                            convertedValue = Enum.Parse(underlyingType, filter.Value.ToString()!);
                        else
                            convertedValue = Convert.ChangeType(filter.Value, underlyingType);
                    }
                    catch
                    {
                        continue;
                    }

                    var constant = Expression.Constant(convertedValue, underlyingType);
                    Expression finalConstant = targetType != underlyingType
                        ? Expression.Convert(constant, targetType)
                        : constant;

                    filterExpression = filter.Operator switch
                    {
                        FilterOperator.Equals => Expression.Equal(propertyAccess, finalConstant),

                        FilterOperator.Contains when targetType == typeof(string) => Expression.Call(
                            propertyAccess,
                            typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!,
                            finalConstant),

                        FilterOperator.StartsWith when targetType == typeof(string) => Expression.Call(
                            propertyAccess,
                            typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!,
                            finalConstant),

                        FilterOperator.GreaterThan => Expression.GreaterThan(propertyAccess, finalConstant),
                        FilterOperator.LessThan => Expression.LessThan(propertyAccess, finalConstant),
                        FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyAccess, finalConstant),
                        FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(propertyAccess, finalConstant),
                        _ => Expression.Equal(propertyAccess, finalConstant)
                    };
                }

                combinedExpression = combinedExpression == null
                    ? filterExpression
                    : Expression.AndAlso(combinedExpression, filterExpression);
            }

            if (combinedExpression == null)
                return query;
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return query.Where(lambda);
        }

        public static IQueryable<T> ExcludeColumnFilters<T>(this IQueryable<T> query, IEnumerable<KeyValuePair<string, object>>? filters)
        {
            if (filters == null || !filters.Any()) return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? body = null;

            foreach (var filter in filters)
            {
                var property = Expression.PropertyOrField(parameter, filter.Key);
                var propertyType = property.Type;
                Expression comparison;

                if (filter.Value == null)
                {
                    comparison = Expression.Equal(property, Expression.Constant(null, propertyType));
                }
                else if (propertyType == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                    var value = Expression.Constant(filter.Value?.ToString());
                    comparison = Expression.Call(property, containsMethod, value);
                }
                else
                {
                    var constant = Expression.Constant(Convert.ChangeType(filter.Value, propertyType));
                    comparison = Expression.Equal(property, constant);
                }

                body = body == null ? comparison : Expression.OrElse(body, comparison);
            }

            if (body == null) return query;

            var negatedBody = Expression.Not(body);

            var lambda = Expression.Lambda<Func<T, bool>>(negatedBody, parameter);
            return query.Where(lambda);
        }

        public static IQueryable<T> ApplyGlobalSearch<T>(this IQueryable<T> query, string? search, params Expression<Func<T, string?>>[] properties)
        {
            if (string.IsNullOrWhiteSpace(search) || properties.Length == 0)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? body = null;

            foreach (var property in properties)
            {
                var invoked = Expression.Invoke(property, parameter);
                var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                var searchConstant = Expression.Constant(search);

                var containsExpression = Expression.Call(invoked, containsMethod, searchConstant);

                body = body == null
                    ? containsExpression
                    : Expression.OrElse(body, containsExpression);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(body!, parameter);
            return query.Where(lambda);
        }
        
        public static IQueryable<T> ApplyMultiColumnSearch<T>(this IQueryable<T> query, string? search, Expression<Func<T, string?>>[]? columns)
        {
            if (string.IsNullOrWhiteSpace(search) || columns == null || columns.Length == 0)
                return query;

            var tokens = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? body = null;
            
            foreach (var token in tokens)
            {
                Expression? tokenExpr = null;

                foreach (var column in columns)
                {
                    var columnExpr = Expression.Invoke(column, parameter);
                    var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                    var tokenConstant = Expression.Constant(token, typeof(string));

                    var tokenMatch = Expression.Call(columnExpr, containsMethod, tokenConstant);

                    tokenExpr = tokenExpr == null ? tokenMatch : Expression.OrElse(tokenExpr, tokenMatch);
                }

                body = body == null ? tokenExpr : Expression.AndAlso(body, tokenExpr);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(body!, parameter);
            return query.Where(lambda);
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, bool descending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            var parameter = Expression.Parameter(typeof(T), "e");
            Expression propertyAccess;

            if(!sortBy.Contains('.'))
            {
                try
                {
                    propertyAccess = Expression.PropertyOrField(parameter, sortBy);
                }
                catch (ArgumentException)
                {
                    return query;
                }
            }
            else
            {
                propertyAccess = parameter;
                var parts = sortBy.Split('.');
                try
                {
                    foreach (var member in parts)
                    {
                        propertyAccess = Expression.PropertyOrField(propertyAccess, member);
                    }
                }
                catch (ArgumentException)
                {
                    return query;
                }
            }

            var lambda = Expression.Lambda(propertyAccess, parameter);
            string methodName = descending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), propertyAccess.Type },
                query.Expression,
                Expression.Quote(lambda)
            );

            return query.Provider.CreateQuery<T>(resultExpression);
        }

        public static Expression<Func<TEntity, TProperty>> BuildCaseExpression<TEntity, TKey, TProperty>(
            string keyPropertyName,
            string targetPropertyName,
            Dictionary<TKey, TProperty> updates)
        {
            if (updates == null || updates.Count == 0)
                throw new ArgumentException("Updates dictionary cannot be empty");

            var parameter = Expression.Parameter(typeof(TEntity), "entity");

            var keyProperty = Expression.Property(parameter, keyPropertyName);
            var targetProperty = Expression.Property(parameter, targetPropertyName);

            Expression currentExpression = targetProperty;
            foreach(var kvp in updates)
            {
                var testExpression = Expression.Equal(keyProperty, Expression.Constant(kvp.Key, typeof(TKey)));
                var ifTrueExpression = Expression.Constant(kvp.Value, typeof(TProperty));
                currentExpression = Expression.Condition(testExpression, ifTrueExpression, currentExpression);
            }
            return Expression.Lambda<Func<TEntity, TProperty>>(currentExpression, parameter);
        }
    }
}