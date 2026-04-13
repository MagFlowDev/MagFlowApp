using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Security.Requirements
{
    public class RoleOrPermissionRequirement : IAuthorizationRequirement
    {
        public string Role { get; }
        public string Permission { get; }

        public RoleOrPermissionRequirement(string role, string permission)
        {
            Role = role;
            Permission = permission;
        }
    }
}
