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
                Id = unit.Id
            };
        }
    }
}
