using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Helpers
{
    public static class PluralizationHelper
    {
        public static string Modules(IStringLocalizer localizer, int count)
        {
            if (count == 1)
                return localizer["ModulesOne", count];

            if (count % 10 is >= 2 and <= 4 &&
              !(count % 100 is >= 12 and <= 14))
                return localizer["ModulesFew", count];

            return localizer["ModulesMany", count];
        }
    }
}
