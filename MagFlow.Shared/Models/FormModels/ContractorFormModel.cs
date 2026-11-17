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
        public string Name { get; set; } = string.Empty;
        public string? TaxNumber { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
