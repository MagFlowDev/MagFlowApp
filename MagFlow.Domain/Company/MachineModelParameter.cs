using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.Company
{
    public class MachineModelParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MachineModelId { get; set; }
        [Required]
        public int MachineParameterId { get; set; }
        [Required]
        public string Value { get; set; }
        
        [ForeignKey(nameof(MachineModelId))]
        public MachineModel? MachineModel { get; set; }
        [ForeignKey(nameof(MachineParameterId))]
        public MachineParameter? MachineParameter { get; set; }
    }
}