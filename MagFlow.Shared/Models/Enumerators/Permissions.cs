using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Models.Enumerators
{
    public class Permissions
    {
    }

    [Flags]
    public enum PermissionFlags : long
    {
        None = 0,
        Read = 1 << 0,       // 1
        Create = 1 << 1,     // 2
        Edit = 1 << 2,       // 4
        Delete = 1 << 3,     // 8
        Admin = 1 << 4,      // 16
    }
}
