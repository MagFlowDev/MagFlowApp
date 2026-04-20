using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CompanyScope
{
    public class RoleClaim
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid ClaimId { get; set; }

        [ForeignKey(nameof(ClaimId))]
        public Claim? Claim { get; set; }
    }
}
