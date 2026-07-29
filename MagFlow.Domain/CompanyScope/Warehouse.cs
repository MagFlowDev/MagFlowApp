using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class Warehouse : StatusEntity, IBaseEntity, ICodeEntity, ISoftDeletable, IHistoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; private set; }
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public Enums.WarehouseType Type { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid? RemovedById { get; set; }

        public ICollection<Item> Items { get; set; }
        public ICollection<WarehouseSector> Sectors { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
        [ForeignKey(nameof(RemovedById))]
        public User? RemovedBy { get; set; }

        [NotMapped]
        public Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.Warehouse;

        [NotMapped]
        public ICollection<IEntityHistory> History { get; set; } = [];


        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown,
            Enums.EntityStatus.Active,
            Enums.EntityStatus.Deleted,
        };

        public Warehouse() : base(_allowedStatuses) { }
    }
}