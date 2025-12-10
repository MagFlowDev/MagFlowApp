using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Domain.Core
{
    public class ModulePackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Enums.Currency Currency { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal TotalPricePerMonth { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal TotalPricePerYear { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }
        [Required]
        public DateTime ValidTo { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public ICollection<ModulePricing> Modules { get; set; }
    }
}
