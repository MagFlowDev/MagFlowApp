using MagFlow.Shared.Models.Settings;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Helpers
{
    public static class RelativeTimeHelper
    {
        public static string ToRelativeTime(this DateTime dateTime,
            IStringLocalizer localizer, 
            DateTime? now = null)
        {
            var current = now.HasValue && now.Value > DateTime.MinValue ? now.Value : DateTime.UtcNow;
            var diff = current - dateTime;

            if (diff.TotalSeconds < 120)
                return localizer["TimeJustNow"];

            if (diff.TotalMinutes < 60)
            {
                var minutes = (int)diff.Minutes;
                return localizer["TimeMinuteOneAgo", minutes];
            }

            if(diff.TotalHours < 24)
            {
                var hours = (int)diff.TotalHours;
                return hours == 1
                    ? localizer["TimeHourOneAgo", hours]
                    : localizer["TimeHourManyAgo", hours];
            }

            if(diff.TotalDays <= 7)
            {
                var days = (int)diff.TotalDays;
                return days == 1
                    ? localizer["TimeDayOneAgo", days]
                    : localizer["TimeDayManyAgo", days];
            }

            return localizer["LongTimeAgo"];
        }
    }
}
