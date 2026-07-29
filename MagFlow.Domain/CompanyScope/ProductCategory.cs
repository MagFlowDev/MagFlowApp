using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CompanyScope
{
    public class ProductCategory : IBaseEntity, ICodeEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; private set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsBasic { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime? RemovedAt { get; set; }
    }
}
