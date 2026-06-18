using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ParameterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Enums.ValueType? ValueType { get; set; }
        public UnitDTO? Unit { get; set; }

        public string DropZoneSelector { get; set; } = "available";
        public bool DropZoneHidden { get; set; }
        public int DropZoneOrder { get; set; }
    }
}
