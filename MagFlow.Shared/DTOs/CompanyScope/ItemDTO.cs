using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ItemDTO : IBaseDTO
    {
        public int Id { get; set; }
        public string Code { get; init; }
        public string? ExternalId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? TempQuantity { get; set; }
        public string? Location { get; set; }
        public ProductDTO? Product { get; set; }
        public UnitDTO? Unit { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public UserDTO? CreatedBy { get; set; }
        public Enums.EntityStatus Status { get; set; }

        public List<ItemParameterDTO> Parameters { get; set; } = [];
        public List<ItemComponentDTO> Components { get; set; } = [];
    }
}
