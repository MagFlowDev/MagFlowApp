using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;

namespace MagFlow.Domain.Company
{
    public class FunctionParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public Enums.ValueType ValueType { get; set; }
        [Required]
        public int UnitId { get; set; }
        public string? Description { get; set; }
        
        [ForeignKey(nameof(UnitId))]
        public Unit? Unit { get; set; }
    }
}