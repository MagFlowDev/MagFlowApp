using MagFlow.Shared.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Extensions
{
    public static class StringExtensions
    {
        public static HashSet<char> Consonants = new HashSet<char> { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'w', 'v', 'x', 'z', 
            'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'X', 'T', 'W', 'V', 'X', 'Z' };

        public static string ConsonantsOnly(this string sentence)
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<sentence.Length;i++)
            {
                if (Consonants.Contains(sentence[i]))
                    sb.Append(sentence[i]);
            }
            return sb.ToString();
        }

        public static string? GetCompanyConnectionString(string companyName)
        { 
            var template = AppSettings.ConnectionStrings.CompanyTemplate;
            if(string.IsNullOrEmpty(template) || !template.Contains("Company"))
                return null;
            template = template.Replace("{Company}", "{0}");
            var normalizedCompanyName = new string(companyName.Normalize()
                .Select(c =>
                {
                    return Char.IsWhiteSpace(c) ? '_' : c;
                }).ToArray());
            if (string.IsNullOrEmpty(normalizedCompanyName))
                return null;
            return string.Format(template, normalizedCompanyName);
        }

        public static Guid? ToGuid(this string guidString)
        {
            if(Guid.TryParse(guidString, out var guidResult))
                return guidResult;
            return null;
        }
    }
}
