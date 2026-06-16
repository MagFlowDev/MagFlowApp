using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class ItemFormModel
    {
        public ItemFormGeneralInformation GeneralInformation { get; set; }
        public ItemFormParameterValues ParameterValues { get; set; }
        
        public ItemFormModel()
        {
            GeneralInformation = new ItemFormGeneralInformation();
            ParameterValues = new ItemFormParameterValues();
        }
    }

    public class ItemFormGeneralInformation
    {
        public string Name { get; set; }
    }

    public class ItemFormParameterValues
    {

    }
}
