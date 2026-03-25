using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.Interfaces
{
    public interface ISoftDeletable
    {
        DateTime? RemovedAt { get; set; }
    }
}
