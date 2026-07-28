using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class ProcessStep : StatusEntity, IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProcessId { get; set; }
        [Required]
        public int MachineId { get; set; }
        [Required]
        public int MachineFunctionId { get; set; }
        [Required]
        public int SeqNo { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        [ForeignKey(nameof(ProcessId))]
        public Process? Process { get; set; }
        [ForeignKey(nameof(MachineId))]
        public Machine? Machine { get; set; }
        [ForeignKey(nameof(MachineFunctionId))]
        public MachineFunction? MachineFunction { get; set; }

        public ICollection<ProcessStepIO> StepIO { get; set; }
        public ICollection<ProcessStepParameter> Parameters { get; set; }


        private static readonly HashSet<Enums.EntityStatus> _allowedStatuses = new()
        {
            Enums.EntityStatus.Unknown,
            Enums.EntityStatus.Active,
            Enums.EntityStatus.Deleted,
        };

        public ProcessStep() : base(_allowedStatuses) { }
    }
}