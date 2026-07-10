using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class UnitConversionDTO
    {
        public int? Id { get; set; }

        public UnitDTO FromUnit { get; set; }
        public UnitDTO ToUnit { get; set; }
        public decimal ConversionRate { get; set; }
        public string? Note { get; set; }
    }
}
