using System.Linq.Expressions;

namespace MagFlow.Shared.Models
{

    public class QueryOptions<T>
    {
        public string? Search { get; set; }
        public Expression<Func<T, string?>>[]? SearchColumns { get; set; }

        public Dictionary<string, object>? Filters { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 25;
    }
}