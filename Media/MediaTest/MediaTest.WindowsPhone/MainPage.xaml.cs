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

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
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
