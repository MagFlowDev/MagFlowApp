using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models
{
    public class CodeConfig
    {
        public string Prefix { get; set; } = string.Empty;
        public bool IncludeYear { get; set; }
        public int MinDigits { get; set; } = 4;
    }
}
