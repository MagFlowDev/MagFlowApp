using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.Company
{
    public class ItemParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int ParameterId { get; set; }
        [Required]
        public string Value { get; set; }
        
        [ForeignKey(nameof(ItemId))]
        public Item? Item { get; set; }
        [ForeignKey(nameof(ParameterId))]
        public CustomParameter? Parameter { get; set; }
    }
}