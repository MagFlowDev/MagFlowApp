using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Domain.CompanyScope;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CompanyScope
{
    public class Contractor : StatusEntity, IBaseEntity, ICodeEntity, ISoftDeletable, IHistoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; private set; }
        public string? TaxNumber { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPerson { get; set; }
        public string? Note { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Document> Documents { get; set; }

        public DateTime? RemovedAt { get; set; }

        [NotMapped]
        public override Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.Contractor;


        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown,
            Enums.EntityStatus.Active,
            Enums.EntityStatus.Deleted,
        };

        public Contractor() : base(_allowedStatuses) { }
    }
}
