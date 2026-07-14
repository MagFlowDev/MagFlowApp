using MagFlow.Shared.DTOs.CoreScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class EntityHistoryDTO
    {
        public int Id { get; set; }
        public string? EventType { get; set; }
        public DateTime OccurredAt { get; set; }
        public string? OldValuesJson { get; set; }
        public string? NewValuesJson { get; set; }
        public UserDTO? User { get; set; }
    }
}
