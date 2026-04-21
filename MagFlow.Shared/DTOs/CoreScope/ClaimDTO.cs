using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CoreScope
{
    public class ClaimDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Policy { get; set; }

        public bool ToDelete { get; set; } = false;
        public bool ToAdd { get; set; } = false;
    }
}
