using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MagFlow.BLL.Helpers
{
    public static class CodesGenerator
    {
        private static readonly char[] Base36Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static string GeneratePrefix(string entityName, int minLength = 4, int maxLength = 6)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new ArgumentException("Entity name for code generate cannot be empty");

            string clean = RemoveDiacritics(entityName).ToUpper();
            clean = Regex.Replace(clean, @"[^A-Z0-9]", "");

            int prefixLength = Math.Clamp(clean.Length, minLength, maxLength);
            string prefix = clean.Substring(0, Math.Min(clean.Length, prefixLength));
            if (prefix.Length < minLength)
            {
                prefix = prefix.PadRight(minLength, 'X');
            }

            return prefix;
        }

        public static string GenerateCode(string prefix, long sequenceNumber, int numberPadding = 5)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("Code prefix cannot be empty.", nameof(prefix));
            if (sequenceNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(sequenceNumber), "Sequence/Id number cannot be negative number.");

            string numberPart = ConvertToBase36(sequenceNumber, numberPadding);
            return $"{prefix.ToUpper()}-{numberPart}";
        }



        private static string ConvertToBase36(long value, int padding)
        {
            if (value == 0)
                return new string('0', padding);

            char[] buffer = new char[64];
            int index = buffer.Length;

            while (value > 0)
            {
                buffer[--index] = Base36Alphabet[value % 36];
                value /= 36;
            }

            string result = new string(buffer, index, buffer.Length - index);
            return result.PadLeft(padding, '0');
        }

        private static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
