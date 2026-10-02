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
    public class Stocktake : StatusEntity, IBaseEntity, ISoftDeletable, IHistoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public Enums.StocktakeType Type { get; set; }
        public DateTime PlannedDate { get; set; }
        [Required]
        public int WarehouseId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid? RemovedById { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
        [ForeignKey(nameof(RemovedById))]
        public User? RemovedBy { get; set; }

        [NotMapped]
        public override Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.Stocktake;


        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown,
            Enums.EntityStatus.InProgress,
            Enums.EntityStatus.Confirmed,
        };

        public Stocktake() : base(_allowedStatuses) { }
    }
}
