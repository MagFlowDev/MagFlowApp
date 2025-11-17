using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;

namespace MagFlow.Domain.Company
{
    public class ProcessStep
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
        [Required]
        public Enums.ProcessStatus Status { get; set; }
        
        [ForeignKey(nameof(ProcessId))]
        public Process? Process { get; set; }
        [ForeignKey(nameof(MachineId))]
        public Machine? Machine { get; set; }
        [ForeignKey(nameof(MachineFunctionId))]
        public MachineFunction? MachineFunction { get; set; }
        
    }
}