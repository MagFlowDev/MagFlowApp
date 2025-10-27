using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class CompanyUser
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime AssignedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}
