using Android.Content;
using Android.Widget;
using Plugin.ExternalMaps.Abstractions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace Plugin.ExternalMaps
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class ExternalMapsImplementation : IExternalMaps
    {

        /// <summary>
        /// Navigate to specific latitude and longitude.
        /// </summary>
        /// <param name="name">Label to display</param>
        /// <param name="latitude">Lat</param>
        /// <param name="longitude">Long</param>
        /// <param name="navigationType">Type of navigation</param>
        public Task<bool> NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default)
        {
            var uri = string.Empty;
             if(string.IsNullOrWhiteSpace(name))
                uri = string.Format("http://maps.google.com/maps?&daddr={0},{1}", latitude.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture));
            else
                uri = string.Format("http://maps.google.com/maps?&daddr={0},{1} ({2})", latitude.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture), name);

            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
            intent.SetClassName("com.google.android.apps.maps", "com.google.android.maps.MapsActivity");

            if (TryIntent(intent))
                return Task.FromResult(true);

            var uri2 = string.Empty;
            if (string.IsNullOrWhiteSpace(name))
                uri2 = string.Format("geo:{0},{1}?q={0},{1}", latitude.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture));
            else
                uri2 = string.Format("geo:{0},{1}?q={0},{1}({2})", latitude.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture), name);

            if (TryIntent(new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri2))))
                return Task.FromResult(true);

            if (TryIntent(new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri))))
                return Task.FromResult(true);

            Debug.WriteLine("No map apps found, unable to navigate");
            return Task.FromResult(false);
        }

        private bool TryIntent(Intent intent)
        {
            try
            {
                intent.SetFlags(ActivityFlags.ClearTop);
                intent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
                return true;
            }
            catch (ActivityNotFoundException)
            {
                return false;
            }
        }
        /// <summary>
        /// Navigate to an address
        /// </summary>
        /// <param name="name">Label to display</param>
        /// <param name="street">Street</param>
        /// <param name="city">City</param>
        /// <param name="state">Sate</param>
        /// <param name="zip">Zip</param>
        /// <param name="country">Country</param>
        /// <param name="countryCode">Country Code if applicable</param>
        /// <param name="navigationType">Navigation type</param>
        public Task<bool> NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = string.Empty;


            if (string.IsNullOrWhiteSpace(street))
                street = string.Empty;


            if (string.IsNullOrWhiteSpace(city))
                city = string.Empty;

            if (string.IsNullOrWhiteSpace(state))
                state = string.Empty;


            if (string.IsNullOrWhiteSpace(zip))
                zip = string.Empty;


            if (string.IsNullOrWhiteSpace(country))
                country = string.Empty;

            var uri = String.Format("http://maps.google.com/maps?q={0} {1}, {2} {3} {4}", street, city, state, zip, country);
            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));

            intent.SetClassName("com.google.android.apps.maps", "com.google.android.maps.MapsActivity");


            if (TryIntent(intent))
                return Task.FromResult(true);

            var uri2 = String.Format("geo:0,0?q={0} {1} {2} {3} {4}", street, city, state, zip, country);

            if (TryIntent(new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri2))))
                return Task.FromResult(true);

            if (TryIntent(new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri))))
                return Task.FromResult(true);

            Debug.WriteLine("No map apps found, unable to navigate");
            return Task.FromResult(false);
        }
    }
}