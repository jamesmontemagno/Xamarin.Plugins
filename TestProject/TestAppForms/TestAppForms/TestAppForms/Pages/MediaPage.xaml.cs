using Media.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace TestAppForms.Pages
{
  public partial class MediaPage : ContentPage
  {
    public MediaPage()
    {
      InitializeComponent();
      takePhoto.Clicked += async (sender, args) =>
        {

          if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
          {
            DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            return;
          }

          var file = await CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
            {

              Directory = "Sample",
              Name = "test.jpg"
            });

          if (file == null)
            return;

          DisplayAlert("File Location", file.Path, "OK");

          image.Source = ImageSource.FromStream(() =>
          {
            var stream = file.GetStream();
            file.Dispose();
            return stream;
          }); 
        };

      pickPhoto.Clicked += async (sender, args) =>
        {
          if (!CrossMedia.Current.IsPickPhotoSupported)
          {
            DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
            return;
          }
          Stream stream = null;
          var file = await CrossMedia.Current.PickPhotoAsync().ConfigureAwait(true);


          if (file == null)
            return;

          stream = file.GetStream();
          file.Dispose();

          image.Source = ImageSource.FromStream(()=>stream); 
        };

      takeVideo.Clicked += async (sender, args) =>
        {
          if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
          {
            DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            return;
          }

          var file = await CrossMedia.Current.TakeVideoAsync(new Media.Plugin.Abstractions.StoreVideoOptions
            {
              Name = "video.mp4",
              Directory = "DefaultVideos", 
            });

          if (file == null)
            return;

          DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

          file.Dispose();
        };

      pickVideo.Clicked += async (sender, args) =>
        {
          if (!CrossMedia.Current.IsPickVideoSupported)
          {
            DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
            return;
          }
          var file = await CrossMedia.Current.PickVideoAsync();

          if (file == null)
            return;

          DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
          file.Dispose();
        };
    }
  }
}
