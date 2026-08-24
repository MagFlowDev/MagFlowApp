using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class WarehouseFormModel
    {
        public WarehouseFormGeneralInformation GeneralInformation { get; set; }
        public List<SectorDTO> Sectors { get; set; }

        public WarehouseFormModel()
        {
            GeneralInformation = new WarehouseFormGeneralInformation();
            Sectors = new List<SectorDTO>();
        }
    }

    public class WarehouseFormGeneralInformation
    {
        public string Name { get; set; }
        public Enums.WarehouseType Type { get; set; }

        public WarehouseFormGeneralInformation()
        {
            Type = Enums.WarehouseType.Main;
        }
    }
}
