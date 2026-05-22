using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class MeasurementUnitFormModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }
        public string Symbol { get; set; }

        public decimal? ParentUnitConversionRate { get; set; }

        public List<MeasurementUnitFormModel> RelatedUnits { get; set; }

        public MeasurementUnitFormModel()
        {
            RelatedUnits = new List<MeasurementUnitFormModel>();
        }
    }
}
