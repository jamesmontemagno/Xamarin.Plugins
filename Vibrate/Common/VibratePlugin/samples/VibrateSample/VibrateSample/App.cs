using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace VibrateSample
{
  public class App : Application
  {
    public App()
    {
      var vibrateButton = new Button
      {
        Text = "Vibrate"
      };

      var sliderVibrate = new Slider(0, 10000.0, 500.0);

      vibrateButton.Clicked += (sender, args) =>
      {
        Refractored.Xam.Vibrate.CrossVibrate.Current.Vibration((int)sliderVibrate.Value);
      };

      // The root page of your application
      MainPage = new NavigationPage(new ContentPage
      {
        Content = new StackLayout
        {
          VerticalOptions = LayoutOptions.Center,
          Padding = 50,
          Children = {
						sliderVibrate,
            vibrateButton
					}
        }
      });
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
