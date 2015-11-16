using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geolocator.Plugin;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace PermissionsTest
{
    public partial class GeolocationPage : ContentPage
    {
        public GeolocationPage()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            ((Button) sender).IsEnabled = false;

            try
            {
                var hasPermission = await CrossPermissions.Current.CheckPermission(Permission.Location);
                if (!hasPermission)
                {
                    if(await CrossPermissions.Current.ShouldShowRequestPermissionRationale(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissions(new[] {Permission.Location});
                    hasPermission = results[Permission.Location];
                }

                if (hasPermission)
                {
                    var results = await CrossGeolocator.Current.GetPositionAsync(10000);
                    LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                }
                else
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {

                LabelGeolocation.Text = "Error: " + ex;
            }

            ((Button)sender).IsEnabled = true;
        }
    }
}
