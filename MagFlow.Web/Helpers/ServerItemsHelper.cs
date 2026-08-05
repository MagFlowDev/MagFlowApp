using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;

namespace MagFlow.Web.Helpers
{
    public static class ServerItemsHelper
    {
        public static QueryOptions<T> ApplyFilters<T, TDTO>(this QueryOptions<T> options, ICollection<IFilterDefinition<TDTO>> filters) where TDTO : IBaseDTO
        {
            options.ColumnFilters = new List<ColumnFilter>();
            foreach (var filter in filters)
            {
                var filterOperator = GetSelectedOperator(filter.Operator, filter.FieldType);
                if(filterOperator != Shared.Models.FilterOperator.IsEmpty && filterOperator != Shared.Models.FilterOperator.IsNotEmpty)
                {
                    var propertyName = filter.Column?.Tag?.ToString();
                    if(filter.Value != null && !string.IsNullOrEmpty(propertyName))
                        options.ColumnFilters.Add(ColumnFilter.Create(propertyName, filterOperator, filter.Value));
                }
            }

            return options;
        }

        private static Shared.Models.FilterOperator GetSelectedOperator(string? filterOperator, FieldType type)
        {
            if (Enum.TryParse<Shared.Models.FilterOperator>(filterOperator, out var op))
                return op;

            return type.IsString ? Shared.Models.FilterOperator.Contains : Shared.Models.FilterOperator.Equals;
        }
    }
}
