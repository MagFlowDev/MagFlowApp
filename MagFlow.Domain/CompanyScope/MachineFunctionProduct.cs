using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class MachineFunctionProduct : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MachineModelFunctionId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public Enums.IODirection Direction { get; set; }
        [Required]
        public bool IsRequired { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(MachineModelFunctionId))]
        public MachineModelFunction? MachineModelFunction { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}