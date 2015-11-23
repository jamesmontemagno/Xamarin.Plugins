using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Permissions.Abstractions
{
    /// <summary>
    /// Interface for Permissions
    /// </summary>
    public interface IPermissions
    {
        /// <summary>
        /// Request to see if you should show a rationale for requesting permission
        /// Only on Android
        /// </summary>
        /// <returns>True or false to show rationale</returns>
        /// <param name="permission">Permission to check.</param>
        Task<bool> ShouldShowRequestPermissionRationaleAsync(Permission permission);

        /// <summary>
        /// Determines whether this instance has permission the specified permission.
        /// </summary>
        /// <returns><c>true</c> if this instance has permission the specified permission; otherwise, <c>false</c>.</returns>
        /// <param name="permission">Permission to check.</param>
        Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission);

        /// <summary>
        /// Requests the permissions from the users
        /// </summary>
        /// <returns>The permissions and their status.</returns>
        /// <param name="permissions">Permissions to request.</param>
        Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions);
    }
}
