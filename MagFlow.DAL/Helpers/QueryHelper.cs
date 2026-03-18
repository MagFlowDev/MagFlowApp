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
        
        public static IQueryable<T> ApplyColumnFilters<T>(this IQueryable<T> query, Dictionary<string, object>? filters)
        {
            if (filters == null || filters.Count == 0)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? body = null;

            foreach (var filter in filters)
            {
                var property = Expression.PropertyOrField(parameter, filter.Key);
                var propertyType = property.Type;

                Expression comparison;

                if (propertyType == typeof(string))
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

                body = body == null
                    ? comparison
                    : Expression.AndAlso(body, comparison);
            }

            if (body == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

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
    }
}