using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Utilities
{
    public static class PermissionPacker
    {
        public static string PackPermissionsIntoString(this IEnumerable<Right> permissions)
        {
            return permissions.Aggregate("", (s, permission) => s + (char)permission);
        }

        public static IEnumerable<Right> UnpackPermissionsFromString(this string packedPermissions)
        {
            if (packedPermissions == null)
                throw new ArgumentNullException(nameof(packedPermissions));
            foreach (char character in packedPermissions)
            {
                yield return ((Right)character);
            }
        }

        public static Right? FindPermissionViaName(this string permissionName)
        {
            return Enum.TryParse(permissionName, out Right permission)
                ? (Right?)permission
                : null;
        }

    }
}
