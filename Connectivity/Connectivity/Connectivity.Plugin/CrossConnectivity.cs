using Plugin.Connectivity.Abstractions;
using System;

namespace Plugin.Connectivity
{
    /// <summary>
    /// Cross platform Connectivity implementations
    /// </summary>
    public class CrossConnectivity
    {
        static Lazy<IConnectivity> Implementation = new Lazy<IConnectivity>(() => CreateConnectivity(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IConnectivity Current
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

        static IConnectivity CreateConnectivity()
        {
#if PORTABLE
            return null;
#else
        return new ConnectivityImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }


        /// <summary>
        /// Dispose of everything 
        /// </summary>
        public static void Dispose()
        {
            if (Implementation != null && Implementation.IsValueCreated)
            {
                Implementation.Value.Dispose();

                Implementation = new Lazy<IConnectivity>(() => CreateConnectivity(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
            }
        }
    }
}
