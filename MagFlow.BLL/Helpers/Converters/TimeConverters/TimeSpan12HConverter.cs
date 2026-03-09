using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MagFlow.BLL.Helpers.Converters.TimeConverters
{
    public class TimeSpan12HConverter : MudBlazor.IReversibleConverter<TimeSpan?, string>
    {
        public string Convert(TimeSpan? input)
        {
            if (!input.HasValue) return string.Empty;
            var dt = DateTime.Today.Add(input.Value);
            var format = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            return dt.ToString(format, CultureInfo.CurrentCulture);
        }

        public TimeSpan? ConvertBack(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dt))
                return dt.TimeOfDay;

            return null;
        }
    }
}
