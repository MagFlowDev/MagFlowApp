using MagFlow.Shared.Models;

namespace MagFlow.Shared.DTOs.CoreScope
{
    public class EventLogDTO
    {
        public int Id { get; set; }
        public Enums.EventLogLevel Level { get; set; }
        public Enums.EventLogCategory Category { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }
        public DateTime OccuredAt { get; set; }
        public string IpAddress { get; set; }
    }
}