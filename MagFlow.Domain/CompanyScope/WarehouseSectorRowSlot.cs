using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CompanyScope
{
    public class WarehouseSectorRowSlot : StatusEntity, IBaseEntity, ICodeEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        [Required]
        public int RowId { get; set; }
        public string Code { get; private set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid? RemovedById { get; set; }

        public ICollection<Item> Items { get; set; }

        [ForeignKey(nameof(RowId))]
        public WarehouseSectorRow? Row { get; set; }

        [NotMapped]
        public override Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.WarehouseSectorRowSlot;


        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown,
            Enums.EntityStatus.Active,
            Enums.EntityStatus.Deleted,
        };

        public WarehouseSectorRowSlot() : base(_allowedStatuses) { }
    }
}
