using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ParameterDTO
    {
        public int ParameterId { get; set; }
        public int EntityTableId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Enums.ValueType? ValueType { get; set; }
        public UnitDTO? Unit { get; set; }
        public bool IsRequired { get; set; }

        public string DropZoneSelector { get; set; } = MagFlow.Shared.Constants.Identificators.DropZoneID.AVAILABLE_SELECTOR;
        public bool DropZoneHidden { get; set; }
        public int DropZoneOrder { get; set; }
    }
}
