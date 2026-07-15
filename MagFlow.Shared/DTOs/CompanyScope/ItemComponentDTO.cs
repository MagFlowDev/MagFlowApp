using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ItemComponentDTO
    {
        public ItemDTO Component { get; set; }
        public decimal Quantity { get; set; }
        public bool IsRequired { get; set; }
        public string? Note { get; set; }
    }
}
