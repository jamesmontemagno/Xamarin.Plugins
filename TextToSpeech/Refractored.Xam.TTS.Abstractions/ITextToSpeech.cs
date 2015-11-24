using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.TextToSpeech.Abstractions
{
  /// <summary>
  /// Interface for TTS
  /// </summary>
    public interface ITextToSpeech : IDisposable
    {
      /// <summary>
      /// Initialize TTS
      /// </summary>
      void Init();

      /// <summary>
      /// Speak back text
      /// </summary>
      /// <param name="text">Text to speak</param>
      /// <param name="queue">If you want to chain together speak command or cancel current</param>
      /// <param name="crossLocale">Locale of voice</param>
      /// <param name="pitch">Pitch of voice</param>
      /// <param name="speakRate">Speak Rate of voice (All) (0.0 - 2.0f)</param>
      /// <param name="volume">Volume of voice (iOS/WP) (0.0-1.0)</param>
      void Speak(string text, bool queue = false, CrossLocale? crossLocale = null, float? pitch = null, float? speakRate = null, float? volume = null);
      
      /// <summary>
      /// Get avalid list of installed languages for TTS
      /// </summary>
      /// <returns></returns>
      IEnumerable<CrossLocale> GetInstalledLanguages();
    }
}
