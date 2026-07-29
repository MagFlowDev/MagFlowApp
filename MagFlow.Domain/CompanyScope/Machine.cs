using MagFlow.Shared.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class Machine : IBaseEntity, ICodeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MachineModelId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; private set; }
        public string? Location { get; set; }
        [Required]
        public DateTime InstallationDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        [Required]
        public bool IsActive { get; set; }
        
        [ForeignKey(nameof(MachineModelId))]
        public MachineModel? MachineModel { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }
    }
}