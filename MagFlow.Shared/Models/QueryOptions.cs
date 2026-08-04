using System.Linq.Expressions;

namespace MagFlow.Shared.Models
{

    public class QueryOptions<T>
    {
        public string? Search { get; set; }
        public Expression<Func<T, string?>>[]? SearchColumns { get; set; }

        public List<ColumnFilter>? ColumnFilters { get; set; }

        public List<KeyValuePair<string, object>>? Exludes { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 25;


        public Func<IQueryable<T>, IQueryable<T>>? Includes { get; set; } = null;
    }

    public class ColumnFilter
    {
        public string PropertyName { get; set; } = string.Empty;
        public FilterOperator Operator { get; set; } = FilterOperator.Equals;
        public object? Value { get; set; }

        public static ColumnFilter Create(string propertyName, FilterOperator filterOperator, object? value)
        {
            return new ColumnFilter()
            {
                PropertyName = propertyName,
                Operator = filterOperator,
                Value = value
            };
        }
    }

    public enum FilterOperator
    {
        Equals,
        Contains,
        StartsWith,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        IsEmpty,
        IsNotEmpty
    }
}