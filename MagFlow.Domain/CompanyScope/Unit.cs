using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class Unit : ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string Name { get; set; }

        public int? ParentUnitId { get; set; }
        [Precision(18, 4)]
        public decimal? ParentUnitConversionRate { get; set; }

        [ForeignKey(nameof(ParentUnitId))]
        public Unit? ParentUnit { get; set; }

        public DateTime? RemovedAt { get; set; }

        public ICollection<Unit> RelatedUnits { get; set; }
    }
}