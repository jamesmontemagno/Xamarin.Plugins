using Plugin.Vibrate.Abstractions;
using System;

namespace Plugin.Vibrate
{
    /// <summary>
    /// 
    /// </summary>
    public static class CrossVibrate
    {
      static Lazy<IVibrate> TTS = new Lazy<IVibrate>(() => CreateVibrate(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

      /// <summary>
      /// Current settings to use
      /// </summary>
      public static IVibrate Current
      {
        get
        {
          var ret = TTS.Value;
          if (ret == null)
          {
            throw NotImplementedInReferenceAssembly();
          }
          return ret;
        }
      }

      static IVibrate CreateVibrate()
      {
#if PORTABLE
        return null;
#else
        return new Vibrate();
#endif
      }

      internal static Exception NotImplementedInReferenceAssembly()
      {
        return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Xam.Plugins.Vibrate NuGet package from your main application project in order to reference the platform-specific implementation.");
      }
    }
}
