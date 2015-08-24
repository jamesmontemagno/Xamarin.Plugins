#if __UNIFIED__
using AVFoundation;
using UIKit;
#else
using MonoTouch.AVFoundation;
using MonoTouch.UIKit;
#endif
using Refractored.Xam.TTS.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refractored.Xam.TTS
{
  /// <summary>
  /// Text to speech implemenation iOS
  /// </summary>
  public class TextToSpeech : ITextToSpeech, IDisposable
  {
    AVSpeechSynthesizer speechSynthesizer;
    /// <summary>
    /// Default contstructor. Creates new AVSpeechSynthesizer
    /// </summary>
    public TextToSpeech()
    {
    }

    /// <summary>
    /// Initialize TTS
    /// </summary>
    public void Init()
    {
      speechSynthesizer = new AVSpeechSynthesizer();
    }

    /// <summary>
    /// Speak back text
    /// </summary>
    /// <param name="text">Text to speak</param>
    /// <param name="queue">If you want to chain together speak command or cancel current</param>
    /// <param name="crossLocale">Locale of voice</param>
    /// <param name="pitch">Pitch of voice</param>
    /// <param name="speakRate">Speak Rate of voice (All) (0.0 - 2.0f)</param>
    /// <param name="volume">Volume of voice (iOS/WP) (0.0-1.0)</param>
    public void Speak(string text, bool queue = false, CrossLocale? crossLocale = null, float? pitch = null, float? speakRate = null, float? volume = null)
    {
      
      if (string.IsNullOrWhiteSpace(text))
        return;

      if (speechSynthesizer == null)
        Init();

      var localCode = crossLocale.HasValue &&
                      !string.IsNullOrWhiteSpace(crossLocale.Value.Language) ?
                      crossLocale.Value.Language :
                      AVSpeechSynthesisVoice.CurrentLanguageCode;

        
      pitch = !pitch.HasValue ? 1.0f : pitch;

      if (!volume.HasValue)
        volume = 1.0f;
      else if (volume > 1.0f)
        volume = 1.0f;
      else if (volume < 0.0f)
        volume = 0.0f;

      var divid = UIDevice.CurrentDevice.CheckSystemVersion(8, 0) ? 8.0f : 4.0f;
      if (!speakRate.HasValue)
          speakRate = AVSpeechUtterance.MaximumSpeechRate / divid; //normal speech, default is fast
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

      if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
      {
        var dummyUtterance = new AVSpeechUtterance(" ");
        dummyUtterance.Voice = voice;
        speechSynthesizer.SpeakUtterance(dummyUtterance);
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

    /// <summary>
    /// Get all installed and valid languages
    /// </summary>
    /// <returns></returns>
    public IEnumerable<CrossLocale> GetInstalledLanguages()
    {
      return AVSpeechSynthesisVoice.GetSpeechVoices()
        .OrderBy(a => a.Language)
        .Select(a => new CrossLocale { Language = a.Language, DisplayName = a.Language });
    }

    /// <summary>
    /// Dispose of TTS
    /// </summary>
    public void Dispose()
    {
      if(speechSynthesizer != null)
      {
        speechSynthesizer.Dispose();
        speechSynthesizer = null;
      }
    }
  }
}
