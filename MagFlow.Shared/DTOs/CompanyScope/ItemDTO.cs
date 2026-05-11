using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Shared.Models;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public Enums.ItemStatus Status { get; set; }
    }
}
