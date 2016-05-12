using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugin.Permissions.Abstractions
{
    public abstract class PermissionsBase : IPermissions
    {
        public abstract Task<bool> ShouldShowRequestPermissionRationaleAsync(Permission permission);

        public abstract Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission);

        public abstract Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions);

        public async Task<PermissionsResult> HasPermissionAsync(params Permission[] requests)
        {
            List<Permission> missing = new List<Permission>();
            foreach (var permission in requests)
            {
                var status = await CheckPermissionStatusAsync(permission).ConfigureAwait(false);
                if (status != PermissionStatus.Granted)
                {
                    // Can't use console as in other parts of the plugin here, would require different design.
                    Debug.WriteLine($"Currently does not have {Enum.GetName(typeof(Permission), permission)} permissions, requesting permission.");
                    missing.Add(permission);
                }
            }
            if (missing.Count == 0) { return PermissionsResult.AllGranted(requests); }

            return new PermissionsResult(await RequestPermissionsAsync(missing.ToArray()));
        }
    }
}
