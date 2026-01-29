using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MinimumRoleAttribute : Attribute
    {
        public string Role { get; set; }
        public MinimumRoleAttribute(string role) => Role = role;
    }
}
