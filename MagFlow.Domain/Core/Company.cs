using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string NormalizedName { get; set; }
        [Required]
        public string ConnectionString { get; set; }
        [Required]
        public string NIP {  get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public ICollection<CompanyUser> Users { get; set; }
        public ICollection<CompanyModule> Modules { get; set; }
        public ICollection<CompanyNotification> Notifications { get; set; }
    }
}
