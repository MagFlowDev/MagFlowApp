using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }

        public ICollection<ApplicationUserRole> Users { get; set; }
    }
}
