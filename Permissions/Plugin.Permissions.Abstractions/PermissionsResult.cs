using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Permissions.Abstractions
{
    public class PermissionsResult
    {
        readonly IDictionary<Permission, PermissionStatus> _requestStatuses;

        public PermissionsResult(IDictionary<Permission, PermissionStatus> requestStatuses)
        {
            _requestStatuses = requestStatuses ?? new Dictionary<Permission, PermissionStatus>();
        }

        /// <summary>
        /// Creates a result with all given permissions granted.
        /// </summary>
        /// <param name="permissions">The permissions to flag as granted.</param>
        /// <returns>A result with all given permissions granted.</returns>
        public static PermissionsResult AllGranted(IEnumerable<Permission> permissions) 
            => new PermissionsResult(permissions.ToDictionary(p => p, _ => PermissionStatus.Granted));

        public static implicit operator bool(PermissionsResult result)
        {
            return result.IsSuccesful;
        }

        public bool IsSuccesful => _requestStatuses.All(permstatus => permstatus.Value == PermissionStatus.Granted);

        public IDictionary<Permission, PermissionStatus> RequestStatuses => _requestStatuses;

        // Not sure if you'd want something like this.
        public IEnumerable<Permission> UnsuccesfulRequests => _requestStatuses.Where(rs => rs.Value != PermissionStatus.Granted).Select(kv => kv.Key);
    }
}
