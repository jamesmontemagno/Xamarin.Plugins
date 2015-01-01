using Android.Content;
using Android.Widget;
using ExternalMaps.Plugin.Abstractions;
using System;


namespace ExternalMaps.Plugin
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
    public void NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default)
    {
      var uri = String.Format("http://maps.google.com/maps?&daddr={0},{1} ({2})", latitude, longitude, name);
      var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
      intent.SetFlags(ActivityFlags.ClearTop);
      intent.SetFlags(ActivityFlags.NewTask);
      intent.SetClassName("com.google.android.apps.maps", "com.google.android.maps.MapsActivity");
      try
      {
        Android.App.Application.Context.StartActivity(intent);
      }
      catch (ActivityNotFoundException)
      {
        try
        {
          var unrestrictedIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
          unrestrictedIntent.SetFlags(ActivityFlags.ClearTop);
          unrestrictedIntent.SetFlags(ActivityFlags.NewTask);
          Android.App.Application.Context.StartActivity(unrestrictedIntent);
        }
        catch (ActivityNotFoundException)
        {
          Toast.MakeText(Android.App.Application.Context, "Please install a maps application", ToastLength.Long).Show();
        }
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
    public void NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default)
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

      var uri = String.Format("http://maps.google.com/maps?q={0} {1}, {2} {3} {4} ({5})", street, city, state, zip, country, name);
      var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
      intent.SetFlags(ActivityFlags.ClearTop);
      intent.SetFlags(ActivityFlags.NewTask);
      intent.SetClassName("com.google.android.apps.maps", "com.google.android.maps.MapsActivity");
      try
      {
        Android.App.Application.Context.StartActivity(intent);
      }
      catch (ActivityNotFoundException)
      {
        try
        {
          var unrestrictedIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
          unrestrictedIntent.SetFlags(ActivityFlags.ClearTop);
          unrestrictedIntent.SetFlags(ActivityFlags.NewTask);
          Android.App.Application.Context.StartActivity(unrestrictedIntent);
        }
        catch (ActivityNotFoundException)
        {
          Toast.MakeText(Android.App.Application.Context, "Please install a maps application", ToastLength.Long).Show();
        }
      }
    }
  }
}