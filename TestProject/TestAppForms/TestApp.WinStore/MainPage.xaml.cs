using DeviceInfo.Plugin;
using Refractored.Xam.TTS;
using Refractored.Xam.TTS.Abstractions;
using Refractored.Xam.Vibrate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestApp.WinStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       IEnumerable<CrossLocale> locales;
        public MainPage()
        {
            this.InitializeComponent();

            locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            ListViewLocales.ItemsSource = locales;

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
          var info = new StringBuilder();
          info.AppendLine("Device Info");
          info.AppendLine("Generated Id: " + CrossDeviceInfo.Current.GenerateAppId());
          info.AppendLine("Generated Id: " + CrossDeviceInfo.Current.GenerateAppId(true));
          info.AppendLine("Generated Id: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello"));
          info.AppendLine("Generated Id: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello", "world"));
          info.AppendLine("Id: " + CrossDeviceInfo.Current.Id);
          info.AppendLine("Model: " + CrossDeviceInfo.Current.Model);
          info.AppendLine("Platform: " + CrossDeviceInfo.Current.Platform);
          info.AppendLine("Version: " + CrossDeviceInfo.Current.Version);
          DeviceInfo.Text = info.ToString();
        }

        private void ButtonSpeak_Click(object sender, RoutedEventArgs e)
        {

          CrossVibrate.Current.Vibration();
          if (string.IsNullOrWhiteSpace(TextBoxSpeak.Text))
            return;

          if(ToggleDefault.IsOn)
          {
            CrossTextToSpeech.Current.Speak(TextBoxSpeak.Text);
            return;
          }

          CrossLocale? locale = ListViewLocales.SelectedItem as CrossLocale?;

          CrossTextToSpeech.Current.Speak(TextBoxSpeak.Text,
            crossLocale: locale,
            pitch: (float)SliderPitch.Value,
            speakRate: (float)SliderSpeakRate.Value,
            volume: (float)SliderVolume.Value);
        }
    }
}
