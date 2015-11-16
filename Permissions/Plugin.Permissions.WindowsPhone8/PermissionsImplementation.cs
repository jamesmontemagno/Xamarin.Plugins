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

        public Task<bool> CheckPermission(Permission permission)
        {
            return Task.FromResult(true);
        }

        public Task<Dictionary<Permission, bool>> RequestPermissions(IEnumerable<Permission> permissions)
        {
            var results = permissions.ToDictionary(permission => permission, permission => true);
            return Task.FromResult(results);
        }
    }
}