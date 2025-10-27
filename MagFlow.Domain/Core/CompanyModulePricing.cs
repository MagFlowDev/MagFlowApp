using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class CompanyModulePricing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid CompanyModuleId { get; set; }
        [Required]
        public Enums.Currency Currency {  get; set; }
        [Required]
        [Precision(8,2)]
        public decimal PricePerMonth { get; set; }
        [Required]
        [Precision(8,2)]
        public decimal PricePerYear { get; set; }
        [Required]
        public DateTime AssignedAt { get; set; }
        public string? Notes { get; set; }

        [ForeignKey(nameof(CompanyModuleId))]
        public CompanyModule CompanyModule { get; set; }
    }
}
