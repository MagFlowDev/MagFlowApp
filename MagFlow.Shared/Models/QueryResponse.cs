using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models
{
    public class QueryResponse<T>
    {
        public List<T> Elements { get; set; } = new();
        public int TotalCount { get; set; } = 0;
    }
}
