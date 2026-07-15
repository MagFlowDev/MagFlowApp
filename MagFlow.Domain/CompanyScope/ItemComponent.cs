using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CompanyScope
{
    public class ItemComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ParentId { get; set; }
        [Required]
        public int ComponentId { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        public string? Note { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Item? Parent { get; set; }
        [ForeignKey(nameof(ComponentId))]
        public Item? Component { get; set; }

        public DateTime? RemovedAt { get; set; }
    }
}
