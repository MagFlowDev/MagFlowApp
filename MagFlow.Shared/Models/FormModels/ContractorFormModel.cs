using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class ContractorFormModel
    {
        public ContractorFormGeneralInformation GeneralInformation { get; set; }

        public ContractorFormModel()
        {
            GeneralInformation = new ContractorFormGeneralInformation();
        }
    }

    public class ContractorFormGeneralInformation
    {
        public string Name { get; set; }
    }
}
