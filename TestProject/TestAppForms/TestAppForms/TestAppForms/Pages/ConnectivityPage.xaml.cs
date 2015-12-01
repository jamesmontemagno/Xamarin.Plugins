using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
    public partial class ConnectivityPage : ContentPage
    {
        public ConnectivityPage()
        {
            InitializeComponent();
            connectivityButton.Clicked += async (sender, args) =>
            {
                try
                {
                    connected.Text = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";
                    bandwidths.Text = "Bandwidths: ";
                    foreach (var band in CrossConnectivity.Current.Bandwidths)
                    {
                        bandwidths.Text += band.ToString() + ", ";
                    }
                    connectionTypes.Text = "ConnectionTypes:  ";
                    foreach (var band in CrossConnectivity.Current.ConnectionTypes)
                    {
                        connectionTypes.Text += band.ToString() + ", ";
                    }

                }
                catch (Exception ex)
                {
                    Xamarin.Insights.Report(ex);
                    await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
                }

                try
                {
                    canReach1.Text = await CrossConnectivity.Current.IsReachable(host.Text) ? "Reachable" : "Not reachable";

                }
                catch (Exception ex)
                {
                    Xamarin.Insights.Report(ex);
                    await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");

                }
                try
                {
                    canReach2.Text = await CrossConnectivity.Current.IsRemoteReachable(host2.Text, int.Parse(port.Text)) ? "Reachable" : "Not reachable";

                }
                catch (Exception ex)
                {
                    Xamarin.Insights.Report(ex);
                    await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");

                }
            };

            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                await DisplayAlert("Connectivity Changed", "IsConnected: " + args.IsConnected.ToString(), "OK");
            };
        }
    }
}
