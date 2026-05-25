using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;

namespace MagFlow.Domain.CompanyScope
{
    public class CustomParameter : ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public Enums.ValueType ValueType { get; set; }
        [Required]
        public int UnitId { get; set; }
        
        [ForeignKey(nameof(UnitId))]
        public Unit? Unit { get; set; }

        public DateTime? RemovedAt { get; set; }
    }
}