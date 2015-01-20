using ExternalMaps.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ExternalMapsSample
{
  public class App : Application
  {
    public App()
    {

      var navigateLatLong = new Button
      {
        Text = "Navigate Lat Long"
      };

      navigateLatLong.Clicked += (sender, args) =>
      {
        CrossExternalMaps.Current.NavigateTo("Space Needle", 47.6204, -122.3491);
      };


      var navigateAddress = new Button
      {
        Text = "Navigate Address"
      };

      navigateAddress.Clicked += (sender, args) =>
      {
        CrossExternalMaps.Current.NavigateTo("Xamarin", "394 pacific ave.", "San Francisco", "CA", "94111", "USA", "USA");
      };
      // The root page of your application
      MainPage = new ContentPage
      {
        Content = new StackLayout
        {
          VerticalOptions = LayoutOptions.Center,
          Children = {
							 navigateAddress,
              navigateLatLong,
					}
        }
      };
    }

    protected override void OnStart()
    {
      // Handle when your app starts
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
  }
}
