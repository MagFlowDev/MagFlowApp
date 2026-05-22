using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models.FormModels;

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

        public static UnitDTO ToDTO(this MeasurementUnitFormModel model, UnitDTO? parentUnit = null)
        {
            var dto = new UnitDTO()
            {
                Id = model.Id ?? 0,
                Name = model.Name,
                Symbol = model.Symbol,
                ParentUnitConversionRate = model.ParentUnitConversionRate,
            };
            if (parentUnit != null)
                dto.ParentUnit = parentUnit;
            dto.RelatedUnits = model.RelatedUnits.ToDTO(dto);
            return dto;
        }

        public static List<UnitDTO> ToDTO(this IEnumerable<MeasurementUnitFormModel> models, UnitDTO? parentUnit = null)
        {
            return models.Select(x => x.ToDTO(parentUnit)).ToList();
        }
    }
}
