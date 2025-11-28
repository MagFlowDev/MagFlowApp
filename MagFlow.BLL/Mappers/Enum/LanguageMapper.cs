using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Mappers.Enum
{
    public static class LanguageMapper
    {
        public static string ToLanguageCode(this Enums.Language language)
        {
            switch(language)
            {
                case Enums.Language.Polish: return "pl-PL";
                case Enums.Language.English: return "en-EN";
                default: return "pl-pl";
            }
        }

        public static string GetAvailableLanguageByCode(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                return "pl-PL";
            else if (languageCode.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                return "en-US";
            else
                return "pl-PL";
        }
    }
}
