using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNumericType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(decimal);
        }
    }
}
