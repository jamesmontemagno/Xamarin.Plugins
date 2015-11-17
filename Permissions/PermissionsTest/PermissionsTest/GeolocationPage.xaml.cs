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

        bool busy;
        async void ButtonPermission_OnClicked(object sender, EventArgs e)
        {
            if (busy)
                return;
            
            busy = true;
            ((Button) sender).IsEnabled = false;

            var status = PermissionStatus.Unknown;
            switch (((Button)sender).StyleId)
            {
                case "Calendar":
                    status = await CrossPermissions.Current.HasPermission(Permission.Calendar);
                    break;
                case "Camera":
                    status = await CrossPermissions.Current.HasPermission(Permission.Camera);
                    break;
                case "Contacts":
                    status = await CrossPermissions.Current.HasPermission(Permission.Contacts);
                    break;
                case "Microphone":
                    status = await CrossPermissions.Current.HasPermission(Permission.Microphone);
                    break;
                case "Phone":
                    status = await CrossPermissions.Current.HasPermission(Permission.Phone);
                    break;
                case "Photos":
                    status = await CrossPermissions.Current.HasPermission(Permission.Photos);
                    break;
                case "Reminders":
                    status = await CrossPermissions.Current.HasPermission(Permission.Reminders);
                    break;
                case "Sensors":
                    status = await CrossPermissions.Current.HasPermission(Permission.Sensors);
                    break;
                case "Sms":
                    status = await CrossPermissions.Current.HasPermission(Permission.Sms);
                    break;
                case "Storage":
                    status = await CrossPermissions.Current.HasPermission(Permission.Storage);
                    break;
            }

            await DisplayAlert("Results", status.ToString(), "OK");

            if (status != PermissionStatus.Granted)
            {
                switch (((Button)sender).StyleId)
                {
                    case "Calendar":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Calendar}))[Permission.Calendar];
                        break;
                    case "Camera":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Camera}))[Permission.Camera];
                        break;
                    case "Contacts":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Contacts}))[Permission.Contacts];
                        break;
                    case "Microphone":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Microphone}))[Permission.Microphone];
                        break;
                    case "Phone":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Phone}))[Permission.Phone];
                        break;
                    case "Photos":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Photos}))[Permission.Photos];
                        break;
                    case "Reminders":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Reminders}))[Permission.Reminders];
                        break;
                    case "Sensors":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Sensors}))[Permission.Sensors];
                        break;
                    case "Sms":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Sms}))[Permission.Sms];
                        break;
                    case "Storage":
                        status = (await CrossPermissions.Current.RequestPermissions(new []{Permission.Storage}))[Permission.Storage];
                        break;
                }

                await DisplayAlert("Results", status.ToString(), "OK");

            }

            busy = false;
            ((Button) sender).IsEnabled = true;
        }

        async void Button_OnClicked(object sender, EventArgs e)
        {
            if (busy)
                return;

            busy = true;
            ((Button) sender).IsEnabled = false;

            try
            {
                var status = await CrossPermissions.Current.HasPermission(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if(await CrossPermissions.Current.ShouldShowRequestPermissionRationale(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissions(new[] {Permission.Location});
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    var results = await CrossGeolocator.Current.GetPositionAsync(10000);
                    LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                }
                else if(status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {

                LabelGeolocation.Text = "Error: " + ex;
            }

            ((Button)sender).IsEnabled = true;
            busy = false;
        }
    }
}
