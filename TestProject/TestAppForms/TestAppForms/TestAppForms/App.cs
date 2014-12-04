using Refractored.Xam.TTS;
using Refractored.Xam.TTS.Abstractions;
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
      var button = new Button
      {
        Text = "Speak"
      };

      var sliderPitch = new Slider(0, 2.0, 1.0);
      var sliderRate = new Slider(0, 2.0, 1.0);
      var sliderVolume = new Slider(0, 1.0, 1.0);

      button.Clicked += (sender, args) =>
        {
          CrossTextToSpeech.Current.Speak("Hello, Forms!",
            pitch: (float)sliderPitch.Value,
            speakRate: (float)sliderRate.Value,
            volume: (float)sliderVolume.Value);
        };

      return new ContentPage
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
            button
          }
        }
      };
    }
  }
}
