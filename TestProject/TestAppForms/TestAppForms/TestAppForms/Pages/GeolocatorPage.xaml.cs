using Geolocator.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class GeolocatorPage : ContentPage
  {
    public GeolocatorPage()
    {
      InitializeComponent();
      buttonGetGPS.Clicked += async (sender, args) =>
      {
        var locator = CrossGeolocator.Current;
        locator.DesiredAccuracy = 50;
        labelGPS.Text = "Getting gps";

        var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

        if (position == null)
        {
          labelGPS.Text = "null gps :(";
          return;
        }
        labelGPS.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
          position.Timestamp, position.Latitude, position.Longitude,
          position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);
      };
    }
  }
}
