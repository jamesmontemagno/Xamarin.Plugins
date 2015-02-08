using Media.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class MediaPage : ContentPage
  {
    public MediaPage()
    {
      InitializeComponent();
      takePhoto.Clicked += async (sender, args) =>
        {
          var file = await CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
            {
              DefaultCamera = Media.Plugin.Abstractions.CameraDevice.Rear,
              Directory = "Sample",
              Name = "test.jpg"
            });

          image.Source = ImageSource.FromStream(() =>
          {
            return file.GetStream();
          }); 
        };

      pickPhoto.Clicked += async (sender, args) =>
        {
          var file = await CrossMedia.Current.PickPhotoAsync();
          image.Source = ImageSource.FromStream(()=>
            {
              return file.GetStream();
            }); 
        };
    }
  }
}
