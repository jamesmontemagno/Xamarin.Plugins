using Refractored.Xam.TTS.Abstractions;
using System;
using System.Linq;
using System.Net;
#if NETFX_CORE
using Windows.Media.SpeechSynthesis;
#else
using Windows.Phone.Speech.Synthesis;
#endif

namespace Refractored.Xam.TTS
{
  public class TextToSpeech : ITextToSpeech
  {
    readonly SpeechSynthesizer synth;
    public TextToSpeech()
    {
      synth = new SpeechSynthesizer();
    }

    public void Init()
    {

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
    public async void Speak(string text, bool queue = false, CrossLocale? locale = null, float? pitch = null, float? speakRate = null, float? volume = null)
    {
      if (string.IsNullOrWhiteSpace(text))
        return;

#if !NETFX_CORE
      if (!queue)
        synth.CancelAll();
#endif
      var localCode = string.Empty;

      //nothing fancy needed here
      if(pitch == null && speakRate == null && volume == null)
      {
        if(locale.HasValue)
        {
          localCode = locale.Value.Language;
#if NETFX_CORE
          var voices = from voice in SpeechSynthesizer.AllVoices
                        where (voice.Language == localCode 
                        && voice.Gender.Equals(VoiceGender.Female))
                        select voice;
          synth.Voice =(voices.Any() ? voices.ElementAt(0) : SpeechSynthesizer.DefaultVoice);
       
#else
          var voices = from voice in InstalledVoices.All
                       where (voice.Language == localCode 
                         && voice.Gender.Equals(VoiceGender.Female))
                         select voice;
          synth.SetVoice(voices.Any() ? voices.ElementAt(0) : InstalledVoices.Default);
#endif
        }
        else
        {
#if NETFX_CORE
          synth.Voice = SpeechSynthesizer.DefaultVoice;
#else
          synth.SetVoice(InstalledVoices.Default);
#endif
        }

#if NETFX_CORE
        synth.SynthesizeTextToStreamAsync(text);
#else
        synth.SpeakTextAsync(text);
#endif
        return;
      }
      if(!locale.HasValue)
      {
#if NETFX_CORE
        localCode = SpeechSynthesizer.DefaultVoice.Language;
#else
        localCode = InstalledVoices.Default.Language;
#endif
      }
      else
      {
        localCode = locale.Value.Language;
#if NETFX_CORE
        var voices = from voice in SpeechSynthesizer.AllVoices
                     where (voice.Language == localCode
                     && voice.Gender.Equals(VoiceGender.Female))
                     select voice;
#else
        var voices = from voice in InstalledVoices.All
                     where (voice.Language == localCode 
                         && voice.Gender.Equals(VoiceGender.Female))
                         select voice;
#endif
        if (!voices.Any())
        {
#if NETFX_CORE
          localCode = SpeechSynthesizer.DefaultVoice.Language;
#else
          localCode = InstalledVoices.Default.Language;
#endif
        }
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
      ssmlText += "xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"" + localCode + "\">";
      ssmlText += "<prosody pitch=\""+pitchProsody+"\" volume=\""+volume.Value +"\" rate=\""+ speakRate.Value+"\" >" + text + "</prosody>";
      ssmlText += "</speak>";

#if NETFX_CORE
      synth.SynthesizeSsmlToStreamAsync(ssmlText);
#else
      synth.SpeakSsmlAsync(ssmlText);
#endif
      
    }

    public System.Collections.Generic.IEnumerable<CrossLocale> GetInstalledLanguages()
    {
#if NETFX_CORE
      return SpeechSynthesizer.AllVoices.Select(a => new CrossLocale { Language = a.Language, DisplayName = a.DisplayName });
#else
      return InstalledVoices.All.Select(a => new CrossLocale { Language = a.Language, DisplayName = a.DisplayName });
#endif
    }
  }
}
