using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Domain.Company
{
    public class ProcessDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProcessId { get; set; }
        [Required]
        public int DocumentId { get; set; }

        [ForeignKey(nameof(ProcessId))]
        public Process? Process { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public Document? Document { get; set; }
    }
}
