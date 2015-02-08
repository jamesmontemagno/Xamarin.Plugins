using Battery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class BatteryPage : ContentPage
  {
    public BatteryPage()
    {
      InitializeComponent();
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
    }
  }
}
