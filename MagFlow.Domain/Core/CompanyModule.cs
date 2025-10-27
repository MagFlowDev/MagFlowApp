using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class CompanyModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public Guid ModuleId { get; set; }
        [Required]
        public DateTime EnabledFrom { get; set; }
        [Required]
        public DateTime EnabledTo { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime AssignedAt { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
        [ForeignKey(nameof(ModuleId))]
        public Module? Module { get; set; }

        public ICollection<CompanyModulePricing> ModulePricings { get; set; }
    }
}
