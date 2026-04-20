using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Constants
{
    public static class Policies
    {
        public const string ADMIN_ONLY = "AdminOnly";
        public const string COMPANY_ADMIN = "CompanyAdminOnly";

        public const string CAN_ACCESS_USERS = "CanAccessUsers";
        public const string USER_ADD_OR_ADMIN = "User.Edit.OrAdmin";

        public static class Claims
        {
            public const string USER_EDIT = "User.Edit";
            public const string USER_ADD = "User.Add";
            public const string USER_DELETE = "User.Delete";
            public const string USER_ADMIN = "User.Admin";
        }
    }
}
