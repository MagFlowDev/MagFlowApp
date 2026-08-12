using MagFlow.Shared.DTOs;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers
{
    public static class FilterMapper
    {
        public static FilterDefinitionDTO ToDTO<T>(this IFilterDefinition<T> filter)
        {
            var type = typeof(T);
            return new FilterDefinitionDTO()
            {
                ColumnId = filter.Id,
                ColumnField = filter.Column != null ? filter.Column.Tag?.ToString() ?? filter.Column.PropertyName : filter.Column?.Title,
                Operator = filter.Operator,
                Value = filter.Value,
                FilterType = type.AssemblyQualifiedName,
            };
        }

        public static List<FilterDefinitionDTO> ToDTO<T>(this IEnumerable<IFilterDefinition<T>> filters)
        {
            return filters.Select(x => x.ToDTO()).ToList();
        }


        public static IFilterDefinition<T>? ToEntity<T>(this FilterDefinitionDTO dto, MudDataGrid<T> dataGrid)
        {
            var column = dataGrid?.RenderedColumns.FirstOrDefault(c => (c.Tag != null && c.Tag.ToString() == dto.ColumnField) || c.PropertyName == dto.ColumnField);
            if (column == null)
                return null;

            return new FilterDefinition<T>
            {
                Id = dto.ColumnId.HasValue ? dto.ColumnId.Value : Guid.NewGuid(),
                Column = column,
                Operator = dto.Operator,
                Value = dto.Value
            };
        }

        public static List<IFilterDefinition<T>?> ToEntity<T>(this IEnumerable<FilterDefinitionDTO> dtos, MudDataGrid<T> dataGrid)
        {
            return dtos.Select(x => x.ToEntity(dataGrid)).ToList();
        }

    }
}
