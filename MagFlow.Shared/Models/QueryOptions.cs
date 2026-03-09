namespace MagFlow.Shared.Models
{

    public class QueryOptions
    {
        public string? Search { get; set; }

        public Dictionary<string, object>? Filters { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 25;
    }
}