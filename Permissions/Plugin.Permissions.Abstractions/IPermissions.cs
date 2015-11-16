using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Permissions.Abstractions
{
    /// <summary>
    /// Interface for Permissions
    /// </summary>
    public interface IPermissions
    {
        Task<bool> ShouldShowRequestPermissionRationale(Permission permission);
        Task<bool> CheckPermission(Permission permission);
        Task<Dictionary<Permission, bool>> RequestPermissions(IEnumerable<Permission> permissions);
    }
}
