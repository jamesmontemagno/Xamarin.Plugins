using Plugin.Battery;
using System;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
    public partial class BatteryPage : ContentPage
    {
        public BatteryPage()
        {
            InitializeComponent();
            getBatteryButton.Clicked += async (sender, args) =>
            {
                try
                {
                    batteryLevel.Text = "Level: " + CrossBattery.Current.RemainingChargePercent;
                    batteryStatus.Text = "Status: " + CrossBattery.Current.Status.ToString();
                    batteryChargeType.Text = "ChargeType: " + CrossBattery.Current.PowerSource.ToString();
                    batteryIsLow.Text = "IsLow: " + ((CrossBattery.Current.RemainingChargePercent <= 15) ? "YES" : "NO");
                }
                catch (Exception ex)
                {
                    Xamarin.Insights.Report(ex);
                    await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
                }
            };

            try
            {
                CrossBattery.Current.BatteryChanged += (sender, args) =>
                {
                    batteryLevel.Text = "Changed EVENT! Level: " + args.RemainingChargePercent;
                    batteryStatus.Text = "Status: " + args.Status.ToString();
                    batteryChargeType.Text = "ChargeType: " + args.PowerSource.ToString();
                    batteryIsLow.Text = "IsLow: " + ((args.IsLow) ? "YES" : "NO");
                };
            }
            catch
            {
            }
        }
    }
}
