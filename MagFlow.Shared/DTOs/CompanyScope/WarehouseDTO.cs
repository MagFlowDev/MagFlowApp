using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class WarehouseDTO : IBaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; init; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Enums.WarehouseType Type { get; set; }
        public Enums.EntityStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public List<SectorDTO> Sectors { get; set; } = new List<SectorDTO>();
    }

    public class SectorDTO : IBaseDTO
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public string Code { get; init; }
        public Enums.EntityStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public List<RowDTO> Rows { get; set; } = new List<RowDTO>();
    }

    public class RowDTO : IBaseDTO
    {
        public int Id { get; set; }
        public int SectorId { get; set; }
        public string Name { get; set; }
        public string Code { get; init; }
        public Enums.EntityStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public List<SlotDTO> Slots { get; set; } = new List<SlotDTO>();
    }

    public class SlotDTO : IBaseDTO
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public string Name { get; set; }
        public string Code { get; init; }
        public Enums.EntityStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
    }
}
