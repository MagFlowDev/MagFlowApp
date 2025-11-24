using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.Company
{
    public class MachineModelFunction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MachineModelId { get; set; }
        [Required]
        public int MachineFunctionId { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(MachineModelId))]
        public MachineModel? MachineModel { get; set; }
        [ForeignKey(nameof(MachineFunctionId))]
        public MachineFunction? MachineFunction { get; set; }

        public ICollection<MachineFunctionParameter> Parameters { get; set; }
        public ICollection<MachineFunctionProduct> Products { get; set; }

    }
}