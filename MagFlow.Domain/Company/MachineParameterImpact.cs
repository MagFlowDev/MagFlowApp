using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;

namespace MagFlow.Domain.Company
{
    public class MachineParameterImpact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MachineParameterId { get; set; }
        [Required]
        public int MachineFunctionId { get; set; }
        [Required]
        public int UnitId { get; set; }
        [Required]
        public Enums.ImpactType ImpactType { get; set; }
        [Required]
        public string Formula { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(MachineFunctionId))]
        public MachineParameter? MachineParameter { get; set; }
        [ForeignKey(nameof(MachineFunctionId))]
        public MachineFunction? MachineFunction { get; set; }
        [ForeignKey(nameof(UnitId))]
        public Unit? Unit { get; set; }
    }
}