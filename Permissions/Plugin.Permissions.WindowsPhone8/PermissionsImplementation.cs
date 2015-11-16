using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Plugin.Permissions
{
    /// <summary>
    /// Implementation for Permissions
    /// </summary>
    public class PermissionsImplementation : IPermissions
    {
        public Task<bool> ShouldShowRequestPermissionRationale(Permission permission)
        {
            return Task.FromResult(false);
        }

        public Task<PermissionStatus> HasPermission(Permission permission)
        {
            return Task.FromResult(true);
        }

        public Task<Dictionary<Permission, PermissionStatus>> RequestPermissions(IEnumerable<Permission> permissions)
        {
            var results = permissions.ToDictionary(permission => permission, permission => PermissionStatus.Granted);
            return Task.FromResult(results);
        }
    }
}