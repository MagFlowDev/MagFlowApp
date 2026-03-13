using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MagFlow.BLL.Helpers.Auth
{
    public static class PasswordGenerator
    {
        public static string Generate(int length = 12)
        {
            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specials = "!@#$%^&*()_-+=[{]};:<>|./?";

            var chars = new char[length];

            chars[0] = lowers[RandomNumberGenerator.GetInt32(lowers.Length)];
            chars[1] = uppers[RandomNumberGenerator.GetInt32(uppers.Length)];
            chars[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
            chars[3] = specials[RandomNumberGenerator.GetInt32(specials.Length)];

            string all = lowers + uppers + digits + specials;
            for (int i = 4; i < length; i++)
            {
                chars[i] = all[RandomNumberGenerator.GetInt32(all.Length)];
            }
            RandomNumberGenerator.Shuffle(chars);

            return new string(chars);
        }
    }
}
