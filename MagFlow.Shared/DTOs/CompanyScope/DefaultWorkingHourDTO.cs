using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class DefaultWorkingHourDTO
    {
        public int Id { get; set; }

        public Enums.DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }
}
