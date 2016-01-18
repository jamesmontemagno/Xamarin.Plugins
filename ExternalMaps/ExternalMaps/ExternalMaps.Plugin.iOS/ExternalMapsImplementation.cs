using Plugin.ExternalMaps.Abstractions;
#if __UNIFIED__
using CoreLocation;
using Foundation;
using MapKit;
#else
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
#endif
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugin.ExternalMaps
{
    /// <summary>
    /// Implementation for ExternalMaps
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
            if (string.IsNullOrWhiteSpace(name))
                name = string.Empty;

            try
            {

                NSDictionary dictionary = null;
                var mapItem = new MKMapItem(new MKPlacemark(new CLLocationCoordinate2D(latitude, longitude), dictionary));
                mapItem.Name = name;

                MKLaunchOptions launchOptions = null;
                if (navigationType != NavigationType.Default)
                {
                    launchOptions = new MKLaunchOptions
                    {
                        DirectionsMode = navigationType == NavigationType.Driving ? MKDirectionsMode.Driving : MKDirectionsMode.Walking
                    };
                }

                var mapItems = new[] { mapItem };
                MKMapItem.OpenMaps(mapItems, launchOptions);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch maps: " + ex);
                return Task.FromResult(false);
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
        public async Task<bool> NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default)
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


            CLPlacemark[] placemarks = null;
            MKPlacemarkAddress placemarkAddress = null;
            try
            {
                placemarkAddress = new MKPlacemarkAddress
                {
                    City = city,
                    Country = country,
                    State = state,
                    Street = street,
                    Zip = zip,
                    CountryCode = countryCode
                };

                var coder = new CLGeocoder();
                
                placemarks = await coder.GeocodeAddressAsync(placemarkAddress.Dictionary);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get geocode address from address: " + ex);
                return false;
            }

            if ((placemarks?.Length ?? 0) == 0)
            {
                Debug.WriteLine("No locations exist, please check address.");
                return false;
            }

            try
            {
                var placemark = placemarks[0];

                var mapItem = new MKMapItem(new MKPlacemark(placemark.Location.Coordinate, placemarkAddress));
                mapItem.Name = name;

                MKLaunchOptions launchOptions = null;
                if (navigationType != NavigationType.Default)
                {
                    launchOptions = new MKLaunchOptions
                    {
                        DirectionsMode = navigationType == NavigationType.Driving ? MKDirectionsMode.Driving : MKDirectionsMode.Walking
                    };
                }

                var mapItems = new[] { mapItem };
                MKMapItem.OpenMaps(mapItems, launchOptions);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to launch maps: " + ex);
            }

            return true;
        }
    }
}
