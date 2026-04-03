using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class CompanyFormModel
    {
        public CompanyFormGeneralInformation GeneralInformation { get; set; }
        public CompanyFormContactData ContactData { get; set; }
        public CompanyFormModules Modules { get; set; }
        public CompanyFormAdminAccount AdminAccount { get; set; }

        public CompanyFormModel()
        {
            GeneralInformation = new CompanyFormGeneralInformation();
            ContactData = new CompanyFormContactData();
            Modules = new CompanyFormModules();
            AdminAccount = new CompanyFormAdminAccount();
        }
    }

    public class CompanyFormGeneralInformation
    {
        public string Name { get; set; }
        public string TaxNumber { get; set; }
        public Address Address { get; set; }

        public CompanyFormGeneralInformation()
        {
            Address = new Address();
        }
    }

    public class CompanyFormContactData
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }

    public class CompanyFormModules
    {
        public DateTime? LicenseValidTo { get; set; }
        public IReadOnlyCollection<Guid> SelectedModules { get; set; }

        public CompanyFormModules()
        {
            SelectedModules = new List<Guid>();
        }
    }

    public class CompanyFormAdminAccount
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
