using Refractored.Xam.TTS.Abstractions;
using Refractored.Xam.TTS.Forms.WinPhone;
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Windows.Phone.Speech.Synthesis;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeech))]
namespace Refractored.Xam.TTS.Forms.WinPhone
{
  public class TextToSpeech : ITextToSpeech
  {
    readonly SpeechSynthesizer synth;
    public TextToSpeech()
    {
      synth = new SpeechSynthesizer();
    }

    /// <summary>
    /// Speack back text
    /// </summary>
    /// <param name="text">Text to speak</param>
    /// <param name="queue">If you want to chain together speak command or cancel current</param>
    /// <param name="locale">Locale of voice</param>
    /// <param name="pitch">Pitch of voice</param>
    /// <param name="speakRate">Speak Rate of voice (All) (0.0 - 2.0f)</param>
    /// <param name="volume">Volume of voice (iOS/WP) (0.0-1.0)</param>
    public async void Speak(string text, bool queue = false, string locale = null, float? pitch = null, float? speakRate = null, float? volume = null)
    {
      if (!queue)
        synth.CancelAll();

      //nothing fancy needed here
      if(pitch == null && speakRate == null && volume == null)
      {
        if(locale != null)
        {
          var voices = from voice in InstalledVoices.All
                         where (voice.Language == locale 
                         && voice.Gender.Equals(VoiceGender.Female))
                         select voice;
          synth.SetVoice(voices.Any() ? voices.ElementAt(0) : InstalledVoices.Default);
        }
        else
        {
          synth.SetVoice(InstalledVoices.Default);
        }
        synth.SpeakTextAsync(text);
        return;
      }
      if(locale == null)
      {
        locale = InstalledVoices.Default.Language;
      }
      else
      {
        var voices = from voice in InstalledVoices.All
                     where (voice.Language == locale
                     && voice.Gender.Equals(VoiceGender.Female))
                     select voice;
        if(!voices.Any())
            locale = InstalledVoices.Default.Language;
      }

       if (!volume.HasValue)
        volume = 100.0f;
      else if (volume.Value > 1.0f)
        volume = 100.0f;
      else if (volume.Value < 0.0f)
        volume = 0.0f;
       else
         volume = volume.Value * 100.0f;

      var pitchProsody = "default";
      //var test = "x-low", "low", "medium", "high", "x-high", or "default";
      if(!pitch.HasValue)
        pitchProsody = "default";
      else if(pitch.Value >=  1.6f)
        pitchProsody = "x-high";
      else if(pitch.Value >= 1.1f)
        pitchProsody = "high";
      else if(pitch.Value >= .9f)
        pitchProsody = "medium";
      else if(pitch.Value >= .4f)
        pitchProsody = "low";
      else 
        pitchProsody = "x-low";

     
      string ssmlText = "<speak version=\"1.0\" ";
      ssmlText += "xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\""+locale+"\">";
      ssmlText += "<prosody pitch=\""+pitchProsody+"\" volume=\""+volume.Value +"\" rate=\""+ speakRate.Value+"\" >" + text + "</prosody>";
      ssmlText += "</speak>";
      synth.SpeakSsmlAsync(ssmlText);
      
    }

    public System.Collections.Generic.IEnumerable<string> GetInstalledLanguages()
    {
      return InstalledVoices.All.Select(a => a.Language).Distinct();
    }
  }
}
