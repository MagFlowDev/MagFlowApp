using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ProductParameterDTO : ICodeDTO
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string Code { get; init; }
        public Enums.ValueType? ValueType { get; set; }
        public UnitDTO? Unit { get; set; }
    }
}
