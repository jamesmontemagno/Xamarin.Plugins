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
    public static Page GetMainPage()
    {
      var speakButton = new Button
      {
        Text = "Speak"
      };

      var sliderPitch = new Slider(0, 2.0, 1.0);
      var sliderRate = new Slider(0, 2.0, Device.OnPlatform(.25, 1.0, 1.0));
      var sliderVolume = new Slider(0, 1.0, 1.0);

      speakButton.Clicked += (sender, args) =>
        {
          CrossTextToSpeech.Current.Speak("Hello, Forms!",
            pitch: (float)sliderPitch.Value,
            speakRate: (float)sliderRate.Value,
            volume: (float)sliderVolume.Value);
        };

      var vibrateButton = new Button
      {
        Text = "Vibrate"
      };

      var sliderVibrate = new Slider(0, 10000.0, 500.0);

      vibrateButton.Clicked += (sender, args) =>
        {
          var v = DependencyService.Get<IVibrate>();
          v.Vibration((int)sliderVibrate.Value);
        };

      return new ContentPage
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
              speakButton,
              new Label{ Text = "Vibrate Length"},
              sliderVibrate,
              vibrateButton

            }
          }
        }
      };
    }
  }
}
