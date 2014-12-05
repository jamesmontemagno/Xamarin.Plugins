using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refractored.Xam.TTS.Abstractions
{
    public interface ITextToSpeech
    {
      void Init();
      void Speak(string text, bool queue = false, CrossLocale? locale = null, float? pitch = null, float? speakRate = null, float? volume = null);
      IEnumerable<CrossLocale> GetInstalledLanguages();
    }
}
