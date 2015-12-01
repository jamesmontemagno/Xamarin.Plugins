using Plugin.ExternalMaps.Abstractions;
using System;

namespace Plugin.ExternalMaps
{
  /// <summary>
  /// Cross platform ExternalMaps implemenations
  /// </summary>
  public class CrossExternalMaps
  {
    static Lazy<IExternalMaps> Implementation = new Lazy<IExternalMaps>(() => CreateExternalMaps(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IExternalMaps Current
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

    static IExternalMaps CreateExternalMaps()
    {
#if PORTABLE
        return null;
#else
        return new ExternalMapsImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
