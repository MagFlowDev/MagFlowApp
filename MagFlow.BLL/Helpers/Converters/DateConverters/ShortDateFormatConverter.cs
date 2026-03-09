using MudBlazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MagFlow.BLL.Helpers.Converters.DateConverters
{
    public class ShortDateFormatConverter : IReversibleConverter<DateTime?, string>
    {
        public string Convert(DateTime? input)
        {
            if (!input.HasValue) return string.Empty;
            var format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            return input.Value.ToString(format, CultureInfo.CurrentCulture);
        }

        public DateTime? ConvertBack(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dt))
                return dt;

            return null;
        }
    }
}
