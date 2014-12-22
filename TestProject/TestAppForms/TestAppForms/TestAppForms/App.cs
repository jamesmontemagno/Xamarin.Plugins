using Connectivity.Plugin;
using DeviceInfo.Plugin;
using ImageCircle.Forms.Plugin.Abstractions;
using Refractored.Xam.TTS;
using Refractored.Xam.TTS.Abstractions;
using Refractored.Xam.Vibrate.Abstractions;
using Sms.Plugin;
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

          CrossSms.Current.SendSms("Hello there!", "+16024926689");
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


      var connectivityButton = new Button
      {
        Text = "Connectivity Test"
      };

      var connected = new Label
      {
        Text = "Is Connected: "
      };

      var connectionTypes = new Label
      {
        Text = "Connection Types: "
      };

      var bandwidths = new Label
      {
        Text = "Bandwidths"
      };

      var host = new Entry
      {
        Text = "127.0.0.1"
      };


      var host2 = new Entry
      {
        Text = "montemagno.com"
      };

      var port = new Entry
      {
        Text = "80",
        Keyboard = Keyboard.Numeric
      };

      var canReach1 = new Label
      {
        Text = "Can reach1: "
      };

      var canReach2 = new Label
      {
        Text = "Can reach2: "
      };


      connectivityButton.Clicked += async (sender, args)=>
      {
        connected.Text = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";
        bandwidths.Text = "Bandwidths: ";
        foreach(var band in CrossConnectivity.Current.Bandwidths)
        {
          bandwidths.Text += band.ToString() + ", ";
        }
        connectionTypes.Text = "ConnectionTypes:  ";
        foreach(var band in CrossConnectivity.Current.ConnectionTypes)
        {
          connectionTypes.Text += band.ToString() + ", ";
        }

        try
        {
          canReach1.Text = await CrossConnectivity.Current.IsReachable(host.Text) ? "Reachable" : "Not reachable";
        
        }
        catch(Exception ex)
        {

        }
        try
        {
          canReach2.Text = await CrossConnectivity.Current.IsRemoteReachable(host2.Text, int.Parse(port.Text)) ? "Reachable" : "Not reachable";

        }
        catch(Exception ex)
        {

        }
       

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


      var disposeButton = new Button
      {
        Text = "Dispose Me"
      };

      disposeButton.Clicked += (sender, args) =>
        {
          CrossConnectivity.Dispose();
          CrossTextToSpeech.Dispose();
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
              vibrateButton,
              new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId()},
              new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true)},
              new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello")},
              new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello", "world")},
              new Label{ Text = "Id: " + CrossDeviceInfo.Current.Id},
              new Label{ Text = "Model: " + CrossDeviceInfo.Current.Model},
              new Label{ Text = "Platform: " + CrossDeviceInfo.Current.Platform},
              new Label{ Text = "Version: " + CrossDeviceInfo.Current.Version},
              connectivityButton,
              connected,
              bandwidths,
              connectionTypes,
              host,
              host2,
              port,
              canReach1,
              canReach2,
              disposeButton,
              new StackLayout
              {
                HeightRequest = 300
              },
              new CircleImage
              {
                BorderColor = Color.Pink,
                BorderThickness = 3,
                HeightRequest = 150,
                WidthRequest = 150,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/55/Tamarin_portrait.JPG"))
              },
              new CircleImage
              {
                BorderColor = Color.Purple,
                BorderThickness = 6,
                HeightRequest = 150,
                WidthRequest = 150,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/e/ed/Saguinus_tripartitus_-_Golden-mantled_Tamarin.jpg"))
              },
              new CircleImage
              {
                BorderColor = Color.Yellow,
                BorderThickness = 9,
                HeightRequest = 150,
                WidthRequest = 150,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/53/Golden_Lion_Tamarin_Leontopithecus_rosalia.jpg"))
              }
            }
          }
        }
      };

      CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
        {
          page.DisplayAlert("Connectivity Changed", "IsConnected: " + args.IsConnected.ToString(), "OK");
        };

      return page;
    }
  }
}
