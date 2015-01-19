using Battery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BatterySample
{
  public class App : Application
  {
    public App()
    {


      var getBatteryButton = new Button
      {
        Text = "Get Battery Stats"
      };

      var batteryLevel = new Label
      {
        Text = "Level?"
      };


      var batteryStatus = new Label
      {
        Text = "Status?"
      };

      var batteryChargeType = new Label
      {
        Text = "ChargeType?"
      };


      var batteryIsLow = new Label
      {
        Text = "IsLow"
      };

      getBatteryButton.Clicked += (sender, args) =>
      {
        batteryLevel.Text = "Level: " + CrossBattery.Current.RemainingChargePercent;
        batteryStatus.Text = "Status: " + CrossBattery.Current.Status.ToString();
        batteryChargeType.Text = "ChargeType: " + CrossBattery.Current.PowerSource.ToString();
        batteryIsLow.Text = "IsLow: " + ((CrossBattery.Current.RemainingChargePercent <= 15) ? "YES" : "NO");
      };

      CrossBattery.Current.BatteryChanged += (sender, args) =>
      {
        batteryLevel.Text = "Changed EVENT! Level: " + args.RemainingChargePercent;
        batteryStatus.Text = "Status: " + args.Status.ToString();
        batteryChargeType.Text = "ChargeType: " + args.PowerSource.ToString();
        batteryIsLow.Text = "IsLow: " + ((args.IsLow) ? "YES" : "NO");
      };

      // The root page of your application
      MainPage = new ContentPage
      {
        Content = new StackLayout
        {
          VerticalOptions = LayoutOptions.Center,
          Children = {
						getBatteryButton,
              new Label{ Text = "Battery Level"},
              batteryLevel,
              new Label{ Text = "Battery Status"},
              batteryStatus,
              new Label{ Text = "Battery Charge Type"},
              batteryChargeType,
              new Label{ Text = "Battery is low"},
              batteryIsLow,
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
