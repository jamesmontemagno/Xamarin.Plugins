using Media.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace MediaTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void PickPhoto_Click(object sender, RoutedEventArgs e)
        {
          var file = await CrossMedia.Current.PickPhotoAsync();
          if (file == null)
            return;
          var message = new MessageDialog(file.Path);
          message.ShowAsync();
        }

        private async void TakePhoto_Click(object sender, RoutedEventArgs e)
        {
          var file = await CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
          {

            Directory = "Sample",
            Name = "test.jpg"
          });

          if (file == null)
            return;

          var message = new MessageDialog(file.Path);
          message.ShowAsync();
        }

        private async void TakeVideo_Click(object sender, RoutedEventArgs e)
        {
          var file = await CrossMedia.Current.TakeVideoAsync(new Media.Plugin.Abstractions.StoreVideoOptions
          {

            Directory = "Sample",
            Name = "test.mp4"
          });

          if (file == null)
            return;

          var message = new MessageDialog(file.Path);
          message.ShowAsync();
        }

        private async void PickVideo_Click(object sender, RoutedEventArgs e)
        {
          var file = await CrossMedia.Current.PickVideoAsync();
          if (file == null)
            return;
          var message = new MessageDialog(file.Path);
          message.ShowAsync();
        }
    }
}
