using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class StockMovementDTO : IBaseDTO
    {
        public int Id { get; set; }

        public WarehouseLocationDTO? SourceLocation { get; set; }
        public WarehouseLocationDTO? TargetLocation { get; set; }
    }
}
