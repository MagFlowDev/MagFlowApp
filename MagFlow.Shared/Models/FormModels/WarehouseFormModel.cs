using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class WarehouseFormModel
    {
        public WarehouseFormGeneralInformation GeneralInformation { get; set; }
        public List<WarehouseFormSector> Sectors { get; set; }

        public WarehouseFormModel()
        {
            GeneralInformation = new WarehouseFormGeneralInformation();
            Sectors = new List<WarehouseFormSector>();
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

    public class WarehouseFormSector
    {
        public string Name { get; set; }

        public List<WarehouseFormSectorRow> Rows { get; set; }

        public WarehouseFormSector()
        {
            Rows = new List<WarehouseFormSectorRow>();
        }
    }

    public class WarehouseFormSectorRow
    {
        public string Name { get; set; }

        public List<WarehouseFormSectorRowSlot> Slots { get; set; }

        public WarehouseFormSectorRow()
        {
            Slots = new List<WarehouseFormSectorRowSlot>();
        }
    }

    public class WarehouseFormSectorRowSlot
    {
        public string Name { get; set; }
    }
}
