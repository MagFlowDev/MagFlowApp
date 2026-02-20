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
        public Enums.DecimalSeparator DecimalSeparator { get; set; }
        public Enums.DateFormat DateFormat { get; set; }
        public Enums.TimeFormat TimeFormat { get; set; }
        public Enums.TimeZone TimeZone { get; set; }

        public bool SystemAlertsEnabled { get; set; }
        public bool ProductionNotificationsEnabled { get; set; }
        public bool EmailNotificationsEnabled { get; set; }


        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }
}
