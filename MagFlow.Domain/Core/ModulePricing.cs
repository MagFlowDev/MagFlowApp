using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class ModulePricing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid ModuleId { get; set; }
        [Required]
        public Enums.Currency Currency {  get; set; }
        [Required]
        [Precision(8,2)]
        public decimal PricePerMonth { get; set; }
        [Required]
        [Precision(8,2)]
        public decimal PricePerYear { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }
        [Required]
        public DateTime ValidTo { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(ModuleId))]
        public Module? Module { get; set; }
    }
}
