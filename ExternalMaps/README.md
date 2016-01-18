## ![](Common/external_map_icon.png)External Maps for Xamarin and Windows

  Open external maps to a specific geolocation or address. Option to launch with navigation option on iOS as well.
       

#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xam.Plugin.ExternalMaps [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.ExternalMaps.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.ExternalMaps/)
* Install into your PCL project and Client projects.

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.iOS Unified|Yes|iOS 7+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|Yes|8.1+|
|Windows Store RT|Yes|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|No||

#### Usage
There are two methods that you can call to navigate either with the geolocation lat/long or with a full address to go to.

```csharp
    /// <summary>
    /// Navigate to specific latitude and longitude.
    /// </summary>
    /// <param name="name">Label to display</param>
    /// <param name="latitude">Lat</param>
    /// <param name="longitude">Long</param>
    /// <param name="navigationType">Type of navigation</param>
    Task<bool> NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default);
    
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
    Task<bool> NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default);
```

Examples:

```csharp
var success = await CrossExternalMaps.Current.NavigateTo("Xamarin", "394 pacific ave.", "San Francisco", "CA", "94111", "USA", "USA");
var success = await CrossExternalMaps.Current.NavigateTo("Space Needle", 47.6204, -122.3491);
```     


**Quirks**
* NavigationType only works on iOS and Windows Phone Silverlight (geolocation only). 
* Android will try to launch Google Maps first. If it is not installed then it will ask to see if a map apps is installed. If that doesn't work then it will launch the browser.
* Windows Phone Silverlight: Will attempt to launch external maps app for walk/drive, else launches bing maps.

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
