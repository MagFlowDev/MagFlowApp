using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class WarehouseSector : StatusEntity, IBaseEntity, ICodeEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        [Required]
        public int WarehouseId { get; set; }
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
        public ICollection<WarehouseSectorRow> Rows { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }

        [NotMapped]
        public override Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.WarehouseSector;


        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown,
            Enums.EntityStatus.Active,
            Enums.EntityStatus.Deleted,
        };

        public WarehouseSector() : base(_allowedStatuses) { }
    }
}