using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MustHaveRoleAttribute : Attribute
    {
        public string Role { get; set; }
        public MustHaveRoleAttribute(string role) => Role = role;
    }
}
