using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagFlow.Domain.CompanyScope
{
    public class StocktakeItemParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int ParameterId { get; set; }
        [Required]
        public string Value { get; set; }

        [ForeignKey(nameof(ItemId))]
        public StocktakeItem? Item { get; set; }
        [ForeignKey(nameof(ParameterId))]
        public CustomParameter? Parameter { get; set; }


        public static StocktakeItemParameter CreateParameterCopy(ItemParameter parameter, int itemId = 0)
        {
            return new StocktakeItemParameter()
            {
                ItemId = itemId,
                ParameterId = parameter.ParameterId,
                Value = parameter.Value
            };
        }
    }
}
