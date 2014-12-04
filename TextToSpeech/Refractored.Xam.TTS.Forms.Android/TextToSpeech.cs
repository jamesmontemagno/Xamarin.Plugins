using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Refractored.Xam.TTS.Abstractions;
using Java.Util;
using Android.Speech.Tts;

[assembly: Xamarin.Forms.Dependency(typeof(Refractored.Xam.TTS.Forms.Droid.TextToSpeech))]
namespace Refractored.Xam.TTS.Forms.Droid
{
  public class TextToSpeech : Java.Lang.Object, ITextToSpeech, Android.Speech.Tts.TextToSpeech.IOnInitListener
  {
    Android.Speech.Tts.TextToSpeech speaker;
    string text,language;
    Locale defaultLanguage;
    float pitch, speakRate;
    bool queue;

    public TextToSpeech() { }

    #region IOnInitListener implementation
    public void OnInit(OperationResult status)
    {
      if (status.Equals(OperationResult.Success))
      {
        Speak();
      }
    }
    #endregion


    public void Speak(string text, bool queue = false, string language = null, float? pitch = null, float? speakRate = null, float? volume = null)
    {
      this.text = text;
      this.language = language;
      this.pitch = pitch == null ? 1.0f : pitch.Value;
      this.speakRate = speakRate == null ? 1.0f : speakRate.Value;
      this.queue = queue;

      if (speaker == null)
      {
        speaker = new Android.Speech.Tts.TextToSpeech(Xamarin.Forms.Forms.Context, this);
        defaultLanguage = speaker.Language;
      }
      else
      {
        Speak();
      }
    }

    private void Speak()
    {
      if (!queue && speaker.IsSpeaking)
        speaker.Stop();

      if (language != null)
      {
        var locale = new Locale(language);
        if (speaker.IsLanguageAvailable(locale) == LanguageAvailableResult.Available)
          speaker.SetLanguage(locale);
      }
      else
      {
        speaker.SetLanguage(defaultLanguage);
      }
      
      speaker.SetPitch(pitch);
      speaker.SetSpeechRate(speakRate);
      
      speaker.Speak(text, queue ? QueueMode.Add : QueueMode.Flush, null);
    }

    public IEnumerable<string> GetInstalledLanguages()
    {
      return Locale.GetAvailableLocales().Select(a => a.Language).Distinct();
    }
  }
}
