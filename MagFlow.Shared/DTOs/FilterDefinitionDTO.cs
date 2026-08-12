using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs
{
    public class FilterDefinitionDTO
    {
        public Guid? ColumnId { get; set; }
        public string? ColumnField { get; set; }
        public string? Operator { get; set; }
        public object? Value { get; set; }
        public string? FilterType { get; set; }
    }
}
