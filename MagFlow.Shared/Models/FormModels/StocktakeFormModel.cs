using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class StocktakeFormModel
    {
        public WarehouseDTO Warehouse { get; set; }
        public List<SectorDTO> SelectedSectors { get; set; }
        public Enums.StocktakeType Type { get; set; }
        public DateTime PlannedDate { get; set; }
    }
}
