using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class WarehouseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Enums.WarehouseType Type { get; set; }
        public Enums.WarehouseStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public ICollection<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public ICollection<WarehouseStorageDTO> Storages { get; set; } = new List<WarehouseStorageDTO>();
    }

    public class WarehouseStorageDTO
    {
        public int Id { get; set; }
    }
}
