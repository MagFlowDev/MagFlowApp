using MagFlow.Shared.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MagFlow.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static long GetModulePermissionMask(this ClaimsPrincipal user, string moduleCode)
        {
            var claim = user?.FindFirst($"perms:{moduleCode}")?.Value;
            if (string.IsNullOrEmpty(claim)) return 0;
            return long.TryParse(claim, out var v) ? v : 0;
        }

        public static bool HasModulePermission(this ClaimsPrincipal user, string moduleCode, PermissionFlags required)
        {
            var mask = (PermissionFlags)user.GetModulePermissionMask(moduleCode);
            return (mask & required) == required;
        }

        public static bool HasAnyModulePermission(this ClaimsPrincipal user, string moduleCode, PermissionFlags anyOf)
        {
            var mask = (PermissionFlags)user.GetModulePermissionMask(moduleCode);
            return (mask & anyOf) != PermissionFlags.None;
        }
    }
}
