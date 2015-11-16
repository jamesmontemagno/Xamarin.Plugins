using System;

namespace Plugin.CurrentActivity
{
    /// <summary>
    /// Cross platform CurrentActivity implemenations
    /// </summary>
    public class CrossCurrentActivity
    {
        static Lazy<ICurrentActivity> Implementation = new Lazy<ICurrentActivity>(() => CreateCurrentActivity(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ICurrentActivity Current
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

        static ICurrentActivity CreateCurrentActivity()
        {
#if PORTABLE
        return null;
#else
            return new CurrentActivityImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
