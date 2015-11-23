using Plugin.Permissions.Abstractions;
using System;

namespace Plugin.Permissions
{
    /// <summary>
    /// Cross platform Permissions implemenations
    /// </summary>
    public static class CrossPermissions
    {
        static Lazy<IPermissions> Implementation = new Lazy<IPermissions>(CreatePermissions, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IPermissions Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IPermissions CreatePermissions()
        {
#if PORTABLE
            return null;
#else
            return new PermissionsImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
