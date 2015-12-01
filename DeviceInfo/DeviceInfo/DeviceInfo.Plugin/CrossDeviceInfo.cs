using Plugin.DeviceInfo.Abstractions;
using System;

namespace Plugin.DeviceInfo
{
  /// <summary>
  /// Cross platform DeviceInfo implemenations
  /// </summary>
  public class CrossDeviceInfo
  {
    static Lazy<IDeviceInfo> Implementation = new Lazy<IDeviceInfo>(() => CreateDeviceInfo(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IDeviceInfo Current
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

    static IDeviceInfo CreateDeviceInfo()
    {
#if PORTABLE
        return null;
#else
        return new DeviceInfoImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
