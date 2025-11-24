using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.Company
{
    public class MachineFunctionParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MachineModelFunctionId { get; set; }
        [Required]
        public int FunctionParameterId { get; set; }
        [Required]
        public string MinValue { get; set; }
        [Required]
        public string MaxValue { get; set; }
        [Required]
        public string DefaultValue { get; set; }
        [Required]
        public bool IsRequired { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(MachineModelFunctionId))]
        public MachineModelFunction? MachineModelFunction { get; set; }
        [ForeignKey(nameof(FunctionParameterId))]
        public FunctionParameter? FunctionParameter { get; set; }
    }
}