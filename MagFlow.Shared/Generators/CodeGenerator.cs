using MagFlow.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Helpers.Generators
{
    public static class CodeGenerator
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        public static string Module_GenerateCode(string name)
        {
            if(string.IsNullOrEmpty(name) || name.Length < 3)
            {
                Random random = new Random();
                return new string(Enumerable.Repeat(chars, 3)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            var first = name.Take(1).ToString()!;
            var rest = name.Skip(1).ToString()!;
            return first.ToUpper() + rest.ConsonantsOnly().ToUpper();
        }
    }
}
