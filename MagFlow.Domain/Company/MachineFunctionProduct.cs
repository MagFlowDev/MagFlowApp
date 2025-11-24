using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;

namespace MagFlow.Domain.Company
{
    public class MachineFunctionProduct
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