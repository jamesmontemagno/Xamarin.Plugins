using Plugin.Battery.Abstractions;
using System;

namespace Plugin.Battery
{
  /// <summary>
  /// Cross platform Battery implemenations
  /// </summary>
  public class CrossBattery
  {
    static Lazy<IBattery> Implementation = new Lazy<IBattery>(() => CreateBattery(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IBattery Current
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

    static IBattery CreateBattery()
    {
#if PORTABLE
      return null;
#else
        return new BatteryImplementation();
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
        Implementation = new Lazy<IBattery>(() => CreateBattery(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
      }
    }
  }
}
