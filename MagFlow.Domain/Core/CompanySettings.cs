using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.Core
{
    public class CompanySettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CompanyId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }


        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}
