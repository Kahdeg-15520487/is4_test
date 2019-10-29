using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Dal
{
    public static class PermissionPackers
    {
        public static readonly string DELIMITER = "|";

        public static string PackIntoString(this IEnumerable<string> permissions)
        {
            return permissions.Aggregate("", (s, permission) => s + PermissionPackers.DELIMITER + permission);
        }

        public static IEnumerable<string> UnpackFromString(this string packedPermissions)
        {
            if (packedPermissions == null)
                throw new ArgumentNullException(nameof(packedPermissions));
            return packedPermissions.Split(PermissionPackers.DELIMITER);
        }

    }
}
