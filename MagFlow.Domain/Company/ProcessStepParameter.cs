using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.Domain.Company
{
    public class ProcessStepParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProcessStepId { get; set; }
        [Required]
        public int FunctionParameterId { get; set; }
        [Required]
        [Precision(8,2)]
        public decimal Value { get; set; }
        
        [ForeignKey(nameof(ProcessStepId))]
        public ProcessStep? ProcessStep { get; set; }
        [ForeignKey(nameof(FunctionParameterId))]
        public FunctionParameter? FunctionParameter { get; set; }
    }
}