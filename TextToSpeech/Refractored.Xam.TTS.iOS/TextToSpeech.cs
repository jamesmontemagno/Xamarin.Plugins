#if __UNIFIED__
using AVFoundation;
using UIKit;
#else
using MonoTouch.AVFoundation;
using MonoTouch.UIKit;
#endif
using Plugin.TextToSpeech.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.TextToSpeech
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

            var speechUtterance = GetSpeechUtterance(text, crossLocale, pitch, speakRate, volume);

            SpeakUtterance(queue, speechUtterance);
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

        private AVSpeechUtterance GetSpeechUtterance(string text, CrossLocale? crossLocale, float? pitch, float? speakRate, float? volume)
        {
            AVSpeechUtterance speechUtterance;
            
            var voice = GetVoiceForLocaleLanguage(crossLocale);
            
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                speechUtterance = new AVSpeechUtterance(" ");
                speechUtterance.Voice = voice;                
            }
            else
            {
                speakRate = NormalizeSpeakRate(speakRate);
                volume = NormalizeVolume(volume);
                pitch = NormalizePitch(pitch);

                speechUtterance = new AVSpeechUtterance(text)
                {
                    Rate = speakRate.Value,
                    Voice = voice,
                    Volume = volume.Value,
                    PitchMultiplier = pitch.Value
                };
            }            

            return speechUtterance;
        }

        private AVSpeechSynthesisVoice GetVoiceForLocaleLanguage(CrossLocale? crossLocale)
        {
            var localCode = crossLocale.HasValue &&
                                        !string.IsNullOrWhiteSpace(crossLocale.Value.Language) ?
                                        crossLocale.Value.Language :
                                        AVSpeechSynthesisVoice.CurrentLanguageCode;

            var voice = AVSpeechSynthesisVoice.FromLanguage(localCode);
            if (voice == null)
            {
                Console.WriteLine("Locale not found for voice: " + localCode + " is not valid. Using default.");
                voice = AVSpeechSynthesisVoice.FromLanguage(AVSpeechSynthesisVoice.CurrentLanguageCode);
            }

            return voice;
        }

        private float? NormalizeSpeakRate(float? speakRate)
        {
            var divid = 4.0f;
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0)) //use default .5f
                divid = 2.0f;
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) //use .125f
                divid = 8.0f;
            else
                divid = 4.0f; //use .25f  

            if (!speakRate.HasValue)
                speakRate = AVSpeechUtterance.MaximumSpeechRate / divid; //normal speech, default is fast
            else if (speakRate.Value > AVSpeechUtterance.MaximumSpeechRate)
                speakRate = AVSpeechUtterance.MaximumSpeechRate;
            else if (speakRate.Value < AVSpeechUtterance.MinimumSpeechRate)
                speakRate = AVSpeechUtterance.MinimumSpeechRate;

            return speakRate;
        }

        private static float? NormalizeVolume(float? volume)
        {
            if (!volume.HasValue)
                volume = 1.0f;
            else if (volume > 1.0f)
                volume = 1.0f;
            else if (volume < 0.0f)
                volume = 0.0f;

            return volume;
        }

        private static float? NormalizePitch(float? pitch)
        {
            return pitch.GetValueOrDefault(1.0f);
        }

        private void SpeakUtterance(bool queue, AVSpeechUtterance speechUtterance)
        {
            if (!queue && speechSynthesizer.Speaking)
                speechSynthesizer.StopSpeaking(AVSpeechBoundary.Word);

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }

        /// <summary>
        /// Dispose of TTS
        /// </summary>
        public void Dispose()
        {
            if (speechSynthesizer != null)
            {
                speechSynthesizer.Dispose();
                speechSynthesizer = null;
            }
        }
    }
}
