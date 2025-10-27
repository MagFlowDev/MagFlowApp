using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class CompanyUser
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public DateTime AssignedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}
