using Connectivity.Plugin;
using DeviceInfo.Plugin;
using ExternalMaps.Plugin;
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
using Windows.UI.Popups;
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

          CrossConnectivity.Current.ConnectivityChanged += (sender2, args2) =>
            {
              this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
              {
                var dialog = new MessageDialog("ConnectionChanged: " + args2.IsConnected.ToString());
                dialog.ShowAsync();
              });
            };
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


        private async void Connectivity_Click(object sender, RoutedEventArgs e)
        {
          ConnectivityResults.Text = "Running";
          var builder = new StringBuilder();
          builder.AppendLine(CrossConnectivity.Current.IsConnected ? "Connected" : "Not Connected");
          builder.AppendLine("Connection Types: ");
          foreach (var item in CrossConnectivity.Current.ConnectionTypes)
            builder.AppendLine(item.ToString());

          builder.AppendLine("Bandwidths: ");
          foreach (var item in CrossConnectivity.Current.Bandwidths)
            builder.AppendLine(item.ToString());

          builder.AppendLine((await CrossConnectivity.Current.IsReachable(Address1.Text.Trim())) ? "Reachable" : "Not Reachable");

          builder.AppendLine((await CrossConnectivity.Current.IsReachable(Address2.Text.Trim(), int.Parse(Port.Text))) ? "Reachable" : "Not Reachable");
          ConnectivityResults.Text = builder.ToString();
        }

        private void ButtonNavLatLong_Click(object sender, RoutedEventArgs e)
        {
          CrossExternalMaps.Current.NavigateTo("Space Needle", 47.6204, -122.3491);

        }

        private void ButtonNavAddress_Click(object sender, RoutedEventArgs e)
        {
          CrossExternalMaps.Current.NavigateTo("Xamarin", "394 pacific ave.", "San Francisco", "CA", "94111", "USA", "USA");

        }
    }
}
