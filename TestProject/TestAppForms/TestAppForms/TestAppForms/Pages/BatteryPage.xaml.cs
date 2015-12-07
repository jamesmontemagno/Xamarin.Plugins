using Plugin.Battery;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


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
                    batteryStatus.Text = "Status: " + CrossBattery.Current.Status;
                    batteryChargeType.Text = "Charge Type: " + CrossBattery.Current.PowerSource;
                    batteryIsLow.Text = "IsLow: " + ((CrossBattery.Current.RemainingChargePercent <= 15) ? "YES" : "NO");
                }
                catch (Exception ex)
                {
                    Xamarin.Insights.Report(ex);
                    await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
                }
            };


        }

        void CrossBattery_Current_BatteryChanged (object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs args)
        {
          
            batteryLevel.Text = "Changed EVENT! Level: " + args.RemainingChargePercent;
            batteryStatus.Text = "Status: " + args.Status.ToString();
            batteryChargeType.Text = "Charge Type: " + args.PowerSource.ToString();
            batteryIsLow.Text = "IsLow: " + ((args.IsLow) ? "YES" : "NO");
                
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                CrossBattery.Current.BatteryChanged += CrossBattery_Current_BatteryChanged;
            }
            catch
            {
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                CrossBattery.Current.BatteryChanged -= CrossBattery_Current_BatteryChanged;
            }
            catch
            {
            }
        }
    }
}
