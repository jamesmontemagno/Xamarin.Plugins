using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refractored.Xam.TTS.Abstractions
{
    public interface ITextToSpeech
    {
     
      void Speak(string text, bool queue = false, string locale = null, float? pitch = null, float? speakRate = null, float? volume = null);
      IEnumerable<string> GetInstalledLanguages();
    }
}
