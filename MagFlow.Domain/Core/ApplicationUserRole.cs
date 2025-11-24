using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        [ForeignKey("RoleId")]
        public ApplicationRole? Role { get; set; }
    }
}
