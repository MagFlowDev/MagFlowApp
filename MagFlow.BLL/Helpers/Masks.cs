using MudBlazor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Helpers
{
    public static class Masks
    {
        public static IMask ZIP_CODE = new PatternMask("00-000");
        public static IMask PHONE_NUMBER = new RegexMask(@"^\+?[\d\s.-]{0,20}$");
        public static IMask EMAIL = RegexMask.Email();
        public static IMask WEBSITE = new RegexMask(@"^(https?:\/\/)?[a-zA-Z0-9\-._~:/?#[\]@!$&'()*+,;=%]{0,253}(\/[a-zA-Z0-9._~:/?#[\]@!$&'()*+,;=-]*)?$");
    }
}
