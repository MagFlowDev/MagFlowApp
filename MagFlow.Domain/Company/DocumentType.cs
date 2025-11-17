using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;

namespace MagFlow.Domain.Company
{
    public class DocumentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Enums.DocDirection Direction { get; set; }
        [Required]
        public bool IsFinancial { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}