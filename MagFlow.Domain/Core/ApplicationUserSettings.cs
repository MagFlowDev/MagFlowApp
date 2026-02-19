using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Domain.Core
{
    public class ApplicationUserSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; set; }
        public Enums.Language Language { get; set; }
        public Enums.ThemeMode ThemeMode { get; set; }
        

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }
}
