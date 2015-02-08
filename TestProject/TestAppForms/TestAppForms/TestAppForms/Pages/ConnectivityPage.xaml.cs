using Connectivity.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        try
        {
          canReach1.Text = await CrossConnectivity.Current.IsReachable(host.Text) ? "Reachable" : "Not reachable";

        }
        catch (Exception ex)
        {

        }
        try
        {
          canReach2.Text = await CrossConnectivity.Current.IsRemoteReachable(host2.Text, int.Parse(port.Text)) ? "Reachable" : "Not reachable";

        }
        catch (Exception ex)
        {

        }
      };

      CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
      {
        DisplayAlert("Connectivity Changed", "IsConnected: " + args.IsConnected.ToString(), "OK");
      };
    }
  }
}
