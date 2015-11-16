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
        /// <summary>
        /// Shoulds the show request permission rationale.
        /// iOS never needs to.
        /// </summary>
        /// <returns>The show request permission rationale.</returns>
        /// <param name="permission">Permission.</param>
        public Task<bool> ShouldShowRequestPermissionRationale(Permission permission)
        {
              
            return Task.FromResult(false);
        }

        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <returns>If permission is granted</returns>
        /// <param name="permission">Permission.</param>
        public Task<PermissionStatus> HasPermission(Permission permission)
        {
            switch (permission)
            {
                case Permission.Calendar:
                    break;
                case Permission.Camera:
                    break;
                case Permission.Contacts:
                    break;
                case Permission.Location:
                    break;
                case Permission.Microphone:
                    break;
                case Permission.Notifications:
                    break;
                case Permission.Sensors:
                    break;
                case Permission.Social:
                    break;
            }
            return Task.FromResult(PermissionStatus.Granted);
        }

        public Task<Dictionary<Permission, PermissionStatus>> RequestPermissions(IEnumerable<Permission> permissions)
        {
            var results = permissions.ToDictionary(permission => permission, permission => PermissionStatus.Granted);
            return Task.FromResult(results);
        }
    }
}