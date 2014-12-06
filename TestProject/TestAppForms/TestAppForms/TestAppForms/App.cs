using Refractored.Xam.TTS;
using Refractored.Xam.TTS.Abstractions;
using Refractored.Xam.Vibrate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace TestAppForms
{
  public class App
  {
    static ContentPage page;
    static CrossLocale? locale = null;
    public static Page GetMainPage()
    {
      var speakButton = new Button
      {
        Text = "Speak"
      };

      var languageButton = new Button
      {
        Text = "Default Language"
      };

      var sliderPitch = new Slider(0, 2.0, 1.0);
      var sliderRate = new Slider(0, 2.0, Device.OnPlatform(.25, 1.0, 1.0));
      var sliderVolume = new Slider(0, 1.0, 1.0);

      var useDefaults = new Switch
      {
        IsToggled = false
      };

      speakButton.Clicked += (sender, args) =>
        {
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

      var vibrateButton = new Button
      {
        Text = "Vibrate"
      };

      var sliderVibrate = new Slider(0, 10000.0, 500.0);

      vibrateButton.Clicked += (sender, args) =>
        {
          //var v = DependencyService.Get<IVibrate>();
          //v.Vibration((int)sliderVibrate.Value);
          Refractored.Xam.Vibrate.CrossVibrate.Current.Vibration((int)sliderVibrate.Value);
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
            var selected = await page.DisplayActionSheet("Language", "OK", null, items);
            if (string.IsNullOrWhiteSpace(selected) || selected == "OK")
              return;
            languageButton.Text = selected;
            locale = new CrossLocale { Language = selected };//fine for iOS/WP
          }
        };

      page = new ContentPage
      {
        Content = new ScrollView
        {
          Content = new StackLayout
          {
            Padding = 40,
            Spacing = 10,
            Children = {
              new Label{ Text = "Hello, Forms!"},
              new Label{ Text = "Pitch"},
              sliderPitch,
              new Label{ Text = "Speak Rate"},
              sliderRate,
              new Label{ Text = "Volume"},
              sliderVolume,
              new Label{ Text = "Use Defaults"},
              useDefaults,
              languageButton,
              speakButton,
              new Label{ Text = "Vibrate Length"},
              sliderVibrate,
              vibrateButton
            }
          }
        }
      };

      return page;
    }
  }
}
