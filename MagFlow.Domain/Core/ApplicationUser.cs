using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid DefaultCompanyId { get; set; }

        [ForeignKey(nameof(DefaultCompanyId))]
        public Company? DefaultCompany { get; set; }

        public ICollection<CompanyUser> Companies { get; set; }
        public ICollection<UserSession> Sessions { get; set; }
        public ICollection<UserNotification> Notifications { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
