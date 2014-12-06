using Refractored.Xam.Vibrate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refractored.Xam.Vibrate
{
    public class CrossVibrate
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
        return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Xam.Plugins.TextToSpeech NuGet package from your main application project in order to reference the platform-specific implementation.");
      }
    }
}
