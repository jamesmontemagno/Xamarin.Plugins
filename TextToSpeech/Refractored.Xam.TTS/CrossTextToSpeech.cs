using Plugin.TextToSpeech.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.TextToSpeech
{
  /// <summary>
  /// Cross platform TTS implemenations
  /// </summary>
    public class CrossTextToSpeech
    {
      static Lazy<ITextToSpeech> TTS = new Lazy<ITextToSpeech>(() => CreateTextToSpeech(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

      /// <summary>
      /// Current settings to use
      /// </summary>
      public static ITextToSpeech Current
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

      static ITextToSpeech CreateTextToSpeech()
      {
#if PORTABLE
        return null;
#else
        return new TextToSpeech();
#endif
      }

      internal static Exception NotImplementedInReferenceAssembly()
      {
        return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Xam.Plugins.TextToSpeech NuGet package from your main application project in order to reference the platform-specific implementation.");
      }

      /// <summary>
      /// Dispose of TTS, reset lazy load
      /// </summary>
      public static void Dispose()
      {
        if(TTS.Value != null && TTS.IsValueCreated)
        {
          TTS.Value.Dispose();
          TTS = new Lazy<ITextToSpeech>(() => CreateTextToSpeech(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        }
      }
    }
}
