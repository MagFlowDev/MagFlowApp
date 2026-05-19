using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class UnitMapper
    {
        public static UnitDTO ToDTO(this Unit unit, bool includeRelated = true)
        {
            return new UnitDTO()
            {
                Id = unit.Id,
                Name = unit.Name,
                Symbol = unit.Symbol,
                ParentUnit = unit.ParentUnit?.ToDTO(false),
                ParentUnitConversionRate = unit.ParentUnitConversionRate,
                RelatedUnits = includeRelated ? unit.RelatedUnits?.ToDTO(false) ?? new List<UnitDTO>() : new List<UnitDTO>()
            };
        }

        public static List<UnitDTO> ToDTO(this IEnumerable<Unit> units, bool includeRelated = true)
        {
            return units.Select(x => x.ToDTO(includeRelated)).ToList();
        }
    }
}
