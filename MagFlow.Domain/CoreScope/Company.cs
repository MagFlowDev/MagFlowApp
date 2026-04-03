using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MagFlow.Shared.Models.Interfaces;

namespace MagFlow.Domain.CoreScope
{
    public class Company : ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DbName { get; set; }
        [Required]
        public string NormalizedName { get; set; }
        [Required]
        public string ConnectionString { get; set; }
        [Required]
        public string TaxNumber {  get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public Address Address { get; set; } = new();

        public DateTime? RemovedAt { get; set; }


        public virtual CompanySettings? CompanySettings { get; set; }
        public virtual CompanyLogo? Logo { get; set; }

        public ICollection<CompanyUser> Users { get; set; }
        public ICollection<CompanyModule> Modules { get; set; }
        public ICollection<CompanyNotification> Notifications { get; set; }
    }
}
