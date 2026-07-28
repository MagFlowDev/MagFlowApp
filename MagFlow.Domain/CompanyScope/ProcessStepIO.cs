using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagFlow.Domain.CompanyScope
{
    public class ProcessStepIO : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProcessStepId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public Enums.IODirection Direction { get; set; }
        [Required]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        
        [ForeignKey(nameof(ProcessStepId))]
        public ProcessStep? ProcessStep { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item? Item { get; set; }
    }
}