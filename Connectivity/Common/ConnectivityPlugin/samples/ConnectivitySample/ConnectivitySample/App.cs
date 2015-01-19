using Connectivity.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ConnectivitySample
{
  public class App : Application
  {
    static ContentPage page;
    public App()
    {



      var connectivityButton = new Button
      {
        Text = "Connectivity Test"
      };

      var connected = new Label
      {
        Text = "Is Connected: "
      };

      var connectionTypes = new Label
      {
        Text = "Connection Types: "
      };

      var bandwidths = new Label
      {
        Text = "Bandwidths"
      };

      var host = new Entry
      {
        Text = "127.0.0.1"
      };


      var host2 = new Entry
      {
        Text = "montemagno.com"
      };

      var port = new Entry
      {
        Text = "80",
        Keyboard = Keyboard.Numeric
      };

      var canReach1 = new Label
      {
        Text = "Can reach1: "
      };

      var canReach2 = new Label
      {
        Text = "Can reach2: "
      };


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
        page.DisplayAlert("Connectivity Changed", "IsConnected: " + args.IsConnected.ToString(), "OK");
      };


      // The root page of your application
      MainPage = page = new ContentPage
      {
        Content = new StackLayout
        {
          Padding = 50,
          VerticalOptions = LayoutOptions.Center,
          Children = {
						 connectivityButton,
              connected,
              bandwidths,
              connectionTypes,
              host,
              host2,
              port,
              canReach1,
              canReach2,
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
