using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class StocktakeFormModel
    {
        public int WarehouseId { get; set; }
        public List<int> SelectedSectorIds { get; set; } = [];
        public bool IncludeItemsWithoutSector { get; set; }
        public Enums.StocktakeType? Type { get; set; }
        public DateTime? PlannedDate { get; set; }
    }
}
