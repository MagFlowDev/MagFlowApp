using MagFlow.Shared.Models;
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
        public static string GetDisplayName(this Enum? enumValue, IStringLocalizer localizer)
        {
            if (enumValue == null)
                return string.Empty;

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

        public static string ToDateFormat(this Enums.DateFormat dateFormat)
        {
            return dateFormat switch
            {
                Enums.DateFormat.DD_MM_RRRR_DOTS => "dd.MM.yyyy",
                Enums.DateFormat.MM_DD_RRRR_SLASHES => "MM'/'dd'/'yyyy",
                Enums.DateFormat.RRRR_MM_DD_DASHES => "yyyy-MM-dd",
                Enums.DateFormat.DD_MM_RRRR_DASHES => "dd-MM-yyyy",
                _ => "dd.MM.yyyy"
            };
        }

        public static object? ParseEnum(string enumTypeName, string value)
        {
            var enumType = Type.GetType(enumTypeName);

            if (enumType == null || !enumType.IsEnum)
                return null;

            if (!Enum.TryParse(enumType, value, out var result))
                return null;

            return result;
        }

        public static decimal ToDecimal(this Enums.TaxRate taxRate)
        {
            return taxRate switch
            {
                Enums.TaxRate._0 => 0,
                Enums.TaxRate._5 => 5,
                Enums.TaxRate._8 => 8,
                Enums.TaxRate._23 => 23,
                _ => 0
            };
        }

        public static Enums.TaxRate? ToTaxRate(decimal? taxRate)
        {
            if(!taxRate.HasValue)
                return null;

            return taxRate.Value switch
            {
                0 => Enums.TaxRate._0,
                5 => Enums.TaxRate._5,
                8 => Enums.TaxRate._8,
                23 => Enums.TaxRate._23,
                _ => Enums.TaxRate._0
            };
        }
    }
}
