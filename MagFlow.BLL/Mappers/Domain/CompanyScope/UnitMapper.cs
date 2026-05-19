using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class UnitMapper
    {
        public static UnitDTO ToDTO(this Unit unit)
        {
            return new UnitDTO()
            {
                Id = unit.Id,
                Name = unit.Name,
                Symbol = unit.Symbol,
                ParentUnit = unit.ParentUnit?.ToDTO(),
                ParentUnitConversionRate = unit.ParentUnitConversionRate,
                RelatedUnits = unit.RelatedUnits.ToDTO()
            };
        }

        public static List<UnitDTO> ToDTO(this IEnumerable<Unit> units)
        {
            return units.Select(x => x.ToDTO()).ToList();
        }
    }
}
