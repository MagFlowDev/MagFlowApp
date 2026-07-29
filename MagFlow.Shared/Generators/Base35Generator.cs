using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Generators
{
    public static class Base35Generator
    {
        private static readonly char[] Chars = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private const int MAX_COMBINATIONS = 42875; // 35^3
        private const long PRIME_MULTIPLIER = 27401;

        public static string Convert(int value)
        {
            if (value <= 0)
                return "000";

            return Encode(value - 1);
        }

        public static string ConvertObfuscated(int value)
        {
            long obfuscatedIndex = (value * PRIME_MULTIPLIER) % MAX_COMBINATIONS;
            return Encode(obfuscatedIndex);
        }

        private static string Encode(long index)
        {
            char[] result = new char[3];
            result[0] = Chars[index % 35];
            result[1] = Chars[(index / 35) % 35];
            result[2] = Chars[(index / 1225) % 35];
            return new string(result);
        }
    }
}
