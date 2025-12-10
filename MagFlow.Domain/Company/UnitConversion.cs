using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Domain.Company
{
    public class UnitConversion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int FromUnitId { get; set; }
        [Required]
        public int ToUnitId { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal ConversionRate { get; set; }
        public string? Note { get; set; }

        [ForeignKey(nameof(FromUnitId))]
        public Unit? FromUnit { get; set; }
        [ForeignKey(nameof(ToUnitId))]
        public Unit? ToUnit { get; set; }
    }
}
