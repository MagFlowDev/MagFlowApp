using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.Company
{
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        
        public ICollection<Item> Items { get; set; }
        public ICollection<WarehouseStorage> Storages { get; set; }
    }
}