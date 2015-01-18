using Geolocator.Plugin.Abstractions;
using System;

namespace Geolocator.Plugin
{
  /// <summary>
  /// Cross platform Geolocator implemenations
  /// </summary>
  public class CrossGeolocator
  {
    static Lazy<IGeolocator> Implementation = new Lazy<IGeolocator>(() => CreateGeolocator(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IGeolocator Current
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

    static IGeolocator CreateGeolocator()
    {
#if PORTABLE
        return null;
#else
        return new GeolocatorImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
