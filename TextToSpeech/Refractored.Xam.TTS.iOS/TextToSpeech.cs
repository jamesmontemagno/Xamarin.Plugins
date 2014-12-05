#if __UNIFIED__
using AVFoundation;
#else
using MonoTouch.AVFoundation;
#endif
using Refractored.Xam.TTS.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refractored.Xam.TTS
{
  public class TextToSpeech : ITextToSpeech
  {
    readonly AVSpeechSynthesizer speechSynthesizer;
    public TextToSpeech()
    {
      speechSynthesizer = new AVSpeechSynthesizer();
    }

    public void Init()
    {
    }

    public void Speak(string text, bool queue = false, CrossLocale? locale = null, float? pitch = null, float? speakRate = null, float? volume = null)
    {
      
      if (string.IsNullOrWhiteSpace(text))
        return;

      var localCode = !locale.HasValue ? AVSpeechSynthesisVoice.CurrentLanguageCode : locale.Value.Language;
      pitch = pitch == null ? 1.0f : pitch;

      if (!volume.HasValue)
        volume = 1.0f;
      else if (volume > 1.0f)
        volume = 1.0f;
      else if (volume < 0.0f)
        volume = 0.0f;

      if (!speakRate.HasValue)
        speakRate = AVSpeechUtterance.MaximumSpeechRate / 4; //normal speech, default is fast
      else if (speakRate.Value > AVSpeechUtterance.MaximumSpeechRate)
        speakRate = AVSpeechUtterance.MaximumSpeechRate;
      else if (speakRate.Value < AVSpeechUtterance.MinimumSpeechRate)
        speakRate = AVSpeechUtterance.MinimumSpeechRate;

      var voice = AVSpeechSynthesisVoice.FromLanguage(localCode);
      if(voice == null)
      {
        Console.WriteLine("Locale not found for voice: " + localCode + " is not valid using default.");
        voice = AVSpeechSynthesisVoice.FromLanguage(AVSpeechSynthesisVoice.CurrentLanguageCode);
      }
      var speechUtterance = new AVSpeechUtterance(text)
      {
        Rate = speakRate.Value,
        Voice = voice,
        Volume = volume.Value,
        PitchMultiplier = pitch.Value
      };

      if (!queue && speechSynthesizer.Speaking)
        speechSynthesizer.StopSpeaking(AVSpeechBoundary.Word);

      speechSynthesizer.SpeakUtterance(speechUtterance);
    }

    public IEnumerable<CrossLocale> GetInstalledLanguages()
    {
      return AVSpeechSynthesisVoice.GetSpeechVoices().Select(a => new CrossLocale { Language = a.Language, DisplayName = a.Language});
    }
  }
}
