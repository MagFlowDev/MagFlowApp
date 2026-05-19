using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.Models;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ProductParameterDTO
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Enums.ValueType? ValueType { get; set; }
        public UnitDTO? Unit { get; set; }
    }
}
