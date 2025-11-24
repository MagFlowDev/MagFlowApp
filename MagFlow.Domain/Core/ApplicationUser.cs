using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public Guid? DefaultCompanyId { get; set; }
        [Required]
        public bool IsActive { get; set; }


        [ForeignKey(nameof(DefaultCompanyId))]
        public Company? DefaultCompany { get; set; }

        public ICollection<CompanyUser> Companies { get; set; }
        public ICollection<UserSession> Sessions { get; set; }
        public ICollection<UserNotification> Notifications { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
        public ICollection<EventLog> EventLogs { get; set; }
        public ICollection<ApplicationUserRole> Roles { get; set; }
    }
}
