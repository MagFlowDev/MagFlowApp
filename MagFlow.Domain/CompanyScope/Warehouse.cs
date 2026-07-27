using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class Warehouse : ISoftDeletable, IHistoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public Enums.WarehouseType Type { get; set; }
        [Required]
        public Enums.WarehouseStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid? RemovedById { get; set; }

        public ICollection<Item> Items { get; set; }
        public ICollection<WarehouseStorage> Storages { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
        [ForeignKey(nameof(RemovedById))]
        public User? RemovedBy { get; set; }

        [NotMapped]
        public Enums.HistoryEntityType EntityType => Enums.HistoryEntityType.Warehouse;

        [NotMapped]
        public ICollection<IEntityHistory> History { get; set; } = [];
    }
}