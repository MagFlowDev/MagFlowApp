using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ComponentDTO
    {
        public ProductDTO Product { get; set; }
        public decimal Quantity { get; set; }
        public bool IsRequired { get; set; }
        public string? Note { get; set; }

        public string DropZoneSelector { get; set; } = MagFlow.Shared.Constants.Identificators.DropZoneID.AVAILABLE_SELECTOR;
        public bool DropZoneHidden { get; set; }
        public int DropZoneOrder { get; set; }
    }
}
