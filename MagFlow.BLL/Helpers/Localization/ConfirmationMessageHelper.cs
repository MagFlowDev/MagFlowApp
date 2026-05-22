using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;

namespace MagFlow.BLL.Helpers.Localization
{
    public static class ConfirmationMessageHelper
    {
        public static string GetConfirmationMessage(this IStringLocalizer localizer, string baseConfirmation, int count)
        {
            if (count == 1)
            {
                return localizer[$"{baseConfirmation}_One"];
            }

            int lastDigit = count % 10;
            int lastTwoDigits = count % 100;

            if (lastDigit >= 2 && lastDigit <= 4 && (lastTwoDigits < 12 || lastTwoDigits > 14))
            {
                return string.Format(localizer[$"{baseConfirmation}_Few"], count);
            }

            return string.Format(localizer[$"{baseConfirmation}_Many"], count);
        }
    }
}
