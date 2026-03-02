using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CoreScope
{
    public class CompanyLogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] ImageData { get; set; }
        [Required]
        public string ContentType { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}
