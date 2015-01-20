# Getting Started with External Maps Plugin


#### Usage
There are two methods that you can call to navigate either with the geolocation lat/long or with a full address to go to.

```
    /// <summary>
    /// Navigate to specific latitude and longitude.
    /// </summary>
    /// <param name="name">Label to display</param>
    /// <param name="latitude">Lat</param>
    /// <param name="longitude">Long</param>
    /// <param name="navigationType">Type of navigation</param>
    void NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default);
    
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
    void NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default);
```

Examples:

```
CrossExternalMaps.Current.NavigateTo("Xamarin", "394 pacific ave.", "San Francisco", "CA", "94111", "USA", "USA");
CrossExternalMaps.Current.NavigateTo("Space Needle", 47.6204, -122.3491);
```     


**Quirks**
* NavigationType only works on iOS and Windows Phone Silverlight (geolocation only). 
* Android will try to launch Google Maps first. If it is not installed then it will ask to see if a map apps is installed. If that doesn't work then it will launch the browser.
* Windows Phone Silverlight: Will attempt to launch external maps app for walk/drive, else launches bing maps.
