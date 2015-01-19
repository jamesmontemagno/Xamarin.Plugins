using DeviceInfo.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DeviceInfoSample
{
  public class App : Application
  {
    public App()
    {
      // The root page of your application
      MainPage = new ContentPage
      {
        Content = new StackLayout
        {
          Padding = 50,
          VerticalOptions = LayoutOptions.Center,
          Children = {
						 new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId()},
              new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true)},
              new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello")},
              new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello", "world")},
              new Label{ Text = "Id: " + CrossDeviceInfo.Current.Id},
              new Label{ Text = "Model: " + CrossDeviceInfo.Current.Model},
              new Label{ Text = "Platform: " + CrossDeviceInfo.Current.Platform},
              new Label{ Text = "Version: " + CrossDeviceInfo.Current.Version},
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
