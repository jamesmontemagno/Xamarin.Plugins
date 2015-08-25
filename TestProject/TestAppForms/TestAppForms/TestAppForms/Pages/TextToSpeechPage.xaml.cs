using Refractored.Xam.TTS;
using Refractored.Xam.TTS.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class TextToSpeechPage : ContentPage
  {

    static CrossLocale? locale = null;
    public TextToSpeechPage()
    {
      InitializeComponent();
      sliderRate.Value = Device.OS == TargetPlatform.iOS ? .15f : 1.0f;
      speakButton.Clicked += (sender, args) =>
      {

        //CrossSms.Current.SendSms("Hello there!", "+16024926689");
        var text = "The quick brown fox jumped over the lazy dog.";
        if (useDefaults.IsToggled)
        {
          CrossTextToSpeech.Current.Speak(text);
          return;
        }

        CrossTextToSpeech.Current.Speak(text,
          pitch: (float)sliderPitch.Value,
          speakRate: (float)sliderRate.Value,
          volume: (float)sliderVolume.Value,
          crossLocale: locale);
      };

      languageButton.Clicked += async (sender, args) =>
      {
        var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
        var items = locales.Select(a => a.ToString()).ToArray();

        if (Device.OS == TargetPlatform.Android)
        {
          DependencyService.Get<IDialogs>().DisplayActionSheet("Language", "OK",
              items,
              which =>
              {
                languageButton.Text = items[which];
                locale = locales.ElementAt(which);
              });
        }
        else
        {
          var selected = await DisplayActionSheet("Language", "OK", null, items);
          if (string.IsNullOrWhiteSpace(selected) || selected == "OK")
            return;
          languageButton.Text = selected;
          locale = new CrossLocale { Language = selected };//fine for iOS/WP
        }
      };
    }
  }
}
