using Media.Plugin.Abstractions;
using System;

namespace Media.Plugin
{
    /// <summary>
    /// Cross platform Media implemenations
    /// </summary>
    public class CrossMedia
    {
        static Lazy<IMedia> Implementation = new Lazy<IMedia>(() => CreateMedia(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IMedia Current
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

        static IMedia CreateMedia()
        {
#if PORTABLE
        return null;
#else
            return new MediaImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
