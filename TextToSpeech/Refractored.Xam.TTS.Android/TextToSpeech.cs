using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Refractored.Xam.TTS.Abstractions;
using Java.Util;
using Android.Speech.Tts;
using Android.App;
using Android.OS;

namespace Refractored.Xam.TTS
{
  public class TextToSpeech : Java.Lang.Object, ITextToSpeech, Android.Speech.Tts.TextToSpeech.IOnInitListener
  {
    Android.Speech.Tts.TextToSpeech textToSpeech;
    string text;
    CrossLocale? language;
    Locale defaultLanguage;
    float pitch, speakRate;
    bool queue;
    bool initialized;

    public TextToSpeech() 
    {
    }

    public void Init()
    {
      textToSpeech = new Android.Speech.Tts.TextToSpeech(Application.Context, this);
      defaultLanguage = textToSpeech.Language;
    }

    #region IOnInitListener implementation
    public void OnInit(OperationResult status)
    {
      if (status.Equals(OperationResult.Success))
      {
        initialized = true;
        Speak();
      }
    }
    #endregion


    public void Speak(string text, bool queue = false, CrossLocale? language = null, float? pitch = null, float? speakRate = null, float? volume = null)
    {
      this.text = text;
      this.language = language;
      this.pitch = pitch == null ? 1.0f : pitch.Value;
      this.speakRate = speakRate == null ? 1.0f : speakRate.Value;
      this.queue = queue;

      if (textToSpeech == null || !initialized)
      {
        textToSpeech = new Android.Speech.Tts.TextToSpeech(Application.Context, this);
        defaultLanguage = textToSpeech.Language;
      }
      else
      {
        Speak();
      }
    }

    private void Speak()
    {
      if (string.IsNullOrWhiteSpace(text))
        return;

      if (!queue && textToSpeech.IsSpeaking)
        textToSpeech.Stop();

      if (language.HasValue)
      {
        Locale locale = null;
        if (!string.IsNullOrWhiteSpace(language.Value.Country))
          locale = new Locale(language.Value.Language, language.Value.Country);
        else
          locale = new Locale(language.Value.Language);

        var result = textToSpeech.IsLanguageAvailable(locale);
        if (result == LanguageAvailableResult.CountryAvailable)
        {
          textToSpeech.SetLanguage(locale);
        }
      }
      else
      {
        textToSpeech.SetLanguage(defaultLanguage);
      }
      
      textToSpeech.SetPitch(pitch);
      textToSpeech.SetSpeechRate(speakRate);
      
      textToSpeech.Speak(text, queue ? QueueMode.Add : QueueMode.Flush, null);
    }

    public IEnumerable<CrossLocale> GetInstalledLanguages()
    {
      if (textToSpeech != null && initialized)
      {
       
        if((int)Build.VERSION.SdkInt >= 21)
        {
          try
          {
            //return textToSpeech.AvailableLanguages.Select(a => anew CrossLocale { Country = a.Country, Language = a.Language, DisplayName = a.DisplayName});
          }
          catch(Exception ex)
          {
            Console.WriteLine("Something went horribly wrong, defaulting to old implementation to get languages: " + ex);
          }
        }

        var languages = new List<CrossLocale>();
        var allLocales = Locale.GetAvailableLocales();
        foreach(var locale in allLocales)
        {

          try 
          {
            var result = textToSpeech.IsLanguageAvailable(locale);

            if (result == LanguageAvailableResult.CountryAvailable)
            {
              languages.Add(new CrossLocale { Country = locale.Country, Language = locale.Language, DisplayName = locale.DisplayName});
            }
          }
          catch(Exception ex)
          {
            Console.WriteLine("Error checking language; " + locale + " " + ex);
          }
        }

        return languages;
      }
      else
      {
        return Locale.GetAvailableLocales()
          .Where(a => !string.IsNullOrWhiteSpace(a.Language) && !string.IsNullOrWhiteSpace(a.Country))
          .Select(a => new CrossLocale { Country = a.Country, Language = a.Language, DisplayName = a.DisplayName });
      }
    }
  }
}
