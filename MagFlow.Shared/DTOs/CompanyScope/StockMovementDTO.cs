using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class StockMovementDTO : IBaseDTO
    {
        public int Id { get; set; }

        public Enums.StockMovementType MovementType { get; set; }

        public WarehouseLocationDTO? SourceLocation { get; set; }
        public WarehouseLocationDTO? TargetLocation { get; set; }

        public ContractorDTO? Contractor { get; set; }

        public ItemDTO? Item { get; set; }

        public decimal Quantity { get; set; }

        public UserDTO? User { get; set; }

        public DateTime Date { get; set; }
    }
}
