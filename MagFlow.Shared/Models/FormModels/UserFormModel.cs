using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.FormModels
{
    public class UserFormModel
    {
        public UserFormGeneralInformation GeneralInformation { get; set; }

        public UserFormModel()
        {
            GeneralInformation = new UserFormGeneralInformation();
        }
    }

    public class UserFormGeneralInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
