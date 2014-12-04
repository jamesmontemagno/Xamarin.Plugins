using MonoTouch.AVFoundation;
using Refractored.Xam.TTS.Abstractions;
using Refractored.Xam.TTS.Forms.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeech))]
namespace Refractored.Xam.TTS.Forms.iOS
{
  public class TextToSpeech : ITextToSpeech
  {
    readonly AVSpeechSynthesizer speechSynthesizer;
    public TextToSpeech()
    {
      speechSynthesizer = new AVSpeechSynthesizer();
    }

    public void Speak(string text, bool queue = false, string locale = null, float? pitch = null, float? speakRate = null, float? volume = null)
    {
      

      locale = locale == null ? AVSpeechSynthesisVoice.CurrentLanguageCode : locale;
      pitch = pitch == null ? 1.0f : pitch;

      if (!volume.HasValue)
        volume = 1.0f;
      else if (volume > 1.0f)
        volume = 1.0f;
      else if (volume < 0.0f)
        volume = 0.0f;

      if (!speakRate.HasValue)
        speakRate = AVSpeechUtterance.DefaultSpeechRate;
      else if (speakRate.Value > AVSpeechUtterance.MaximumSpeechRate)
        speakRate = AVSpeechUtterance.MaximumSpeechRate;
      else if (speakRate.Value < AVSpeechUtterance.MinimumSpeechRate)
        speakRate = AVSpeechUtterance.MinimumSpeechRate;

      var speechUtterance = new AVSpeechUtterance(text)
      {
        Rate = speakRate.Value,
        Voice = AVSpeechSynthesisVoice.FromLanguage(locale),
        Volume = volume.Value,
        PitchMultiplier = pitch.Value
      };

      if (!queue && speechSynthesizer.Speaking)
        speechSynthesizer.StopSpeaking(AVSpeechBoundary.Word);

      speechSynthesizer.SpeakUtterance(speechUtterance);
    }

    public IEnumerable<string> GetInstalledLanguages()
    {
      return AVSpeechSynthesisVoice.GetSpeechVoices().Select(a => a.Language).Distinct();
    }
  }
}
