using ExternalMaps.Plugin.Abstractions;
using System;


namespace ExternalMaps.Plugin
{
  /// <summary>
  /// Implementation for ExternalMaps
  /// </summary>
  public class ExternalMapsImplementation : IExternalMaps
  {
    public void NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default)
    {
      throw new NotImplementedException();
    }

    public void NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default)
    {
      throw new NotImplementedException();
    }
  }
}