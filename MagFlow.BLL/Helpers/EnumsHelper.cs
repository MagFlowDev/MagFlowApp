using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Helpers
{
    public static class EnumsHelper
    {
        public static string GetDisplayName(this Enum enumValue, IStringLocalizer localizer)
        {
            FieldInfo? fi = enumValue.GetType().GetField(enumValue.ToString());

            if (fi == null)
                return enumValue.ToString();

            DisplayAttribute[] attributes =
                (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute),
                false);

            if (attributes == null || attributes.Length == 0)
                return enumValue.ToString();

            var name = attributes[0].Name;
            if(string.IsNullOrEmpty(name)) 
                return enumValue.ToString();

            return localizer[name];
        }

    }
}
