using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class UnitDTO : IBaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public UnitDTO? ParentUnit { get; set; }
        public decimal? ParentUnitConversionRate { get; set; }

        public List<UnitDTO> RelatedUnits { get; set; }
    }
}
