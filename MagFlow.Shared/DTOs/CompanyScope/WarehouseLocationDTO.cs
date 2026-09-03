using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class WarehouseLocationDTO
    {
        public WarehouseDTO? Warehouse { get; set; }
        public SectorDTO? Sector { get; set; }
        public RowDTO? Row { get; set; }
        public SlotDTO? Slot { get; set; }
    }
}
