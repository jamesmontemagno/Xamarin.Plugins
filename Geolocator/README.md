## ![](http://www.refractored.com/images/plugin_icon_geolocator.png) Geolocator Plugin for Xamarin and Windows

Simple cross platform plugin to get GPS location including heading, speed, and more. Additionally, you can track geolocation changes :)

Blog Post Walkthrough: https://blog.xamarin.com/geolocation-for-ios-android-and-windows-made-easy/

Ported from [Xamarin.Mobile](http://www.github.com/xamarin/xamarin.mobile) to a cross platform API.

### Setup
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.Geolocator [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.Geolocator.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.Geolocator/)
* Install into your PCL project and Client projects.

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.iOS Unified|Yes|iOS 7+|
|Xamarin.Android|Yes|API 14+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|Yes|8.1+|
|Windows Store RT|Yes|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|No||


### API Usage

Call **CrossGeolocator.Current** from any project or PCL to gain access to APIs.

```csharp

try
{
  var locator = CrossGeolocator.Current;
  locator.DesiredAccuracy = 50;
  
  var position = await locator.GetPositionAsync (timeoutMilliseconds: 10000);
  
  Console.WriteLine ("Position Status: {0}", position.Timestamp);
  Console.WriteLine ("Position Latitude: {0}", position.Latitude);
  Console.WriteLine ("Position Longitude: {0}", position.Longitude);
}
catch(Exception ex)
{
  Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
}
```

### API 

```csharp
/// <summary>
/// Position error event handler
/// </summary>
event EventHandler<PositionErrorEventArgs> PositionError;
```

```csharp
/// <summary>
/// Position changed event handler
/// </summary>
event EventHandler<PositionEventArgs> PositionChanged;
```

```csharp
/// <summary>
/// Desired accuracy in meteres
/// </summary>
double DesiredAccuracy { get; set; }
```

```csharp
/// <summary>
/// Gets if you are listening for location changes
/// </summary>
bool IsListening { get; }
```

```csharp
/// <summary>
/// Gets if device supports heading
/// </summary>
bool SupportsHeading { get; }
```

```csharp
/// <summary>
/// Gets or sets if background updates should be allowed on the geolocator.
/// </summary>
bool AllowsBackgroundUpdates { get; set; }
```

```csharp
/// <summary>
/// Gets or sets if the location updates should be paused automatically (iOS)
/// </summary>
bool PausesLocationUpdatesAutomatically { get; set; }
```

```csharp
/// <summary>
/// Gets if geolocation is available on device
/// </summary>
bool IsGeolocationAvailable { get; }
```

```csharp
/// <summary>
/// Gets if geolocation is enabled on device
/// </summary>
bool IsGeolocationEnabled { get; }
```

```csharp
/// <summary>
/// Gets position async with specified parameters
/// </summary>
/// <param name="timeoutMilliseconds">Timeout in milliseconds to wait, Default Infinite</param>
/// <param name="token">Cancelation token</param>
/// <param name="includeHeading">If you would like to include heading</param>
/// <returns>Position</returns>
Task<Position> GetPositionAsync(int timeoutMilliseconds = Timeout.Infinite, CancellationToken? token = null, bool includeHeading = false);
```

```csharp
/// <summary>
/// Start lisenting for changes
/// </summary>
/// <param name="minTime">Time</param>
/// <param name="minDistance">Distance</param>
/// <param name="includeHeading">Include heading or not</param>
Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false);
```

```csharp
/// <summary>
/// Stop linstening
/// </summary>
Task<bool> StopListeningAsync();
```

### **IMPORTANT**
#### Android:
The ACCESS_COARSE_LOCATION & ACCESS_FINE_LOCATION permissions are required, but the library will automatically add this for you. Additionally, if your users are running Marshmallow the Plugin will automatically prompt them for runtime permissions.

By adding these permissions [Google Play will automatically filter out devices](http://developer.android.com/guide/topics/manifest/uses-feature-element.html#permissions-features) without specific hardward. You can get around this by adding the following to your AssemblyInfo.cs file in your Android project:

```
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]
```

#### iOS:
In iOS 8 you now have to call either RequestWhenInUseAuthorization or RequestAlwaysAuthorization on the location manager (the plugin does this automatically for you, however, need to add either the concisely named NSLocationWhenInUseUsageDescription or NSLocationAlwaysUsageDescription to your Info.plist. 

You will need to add a new string entry called NSLocationWhenInUseUsageDescription or NSLocationAlwaysUsageDescription. 

Go to your info.plist and under source add one of these flags: http://screencast.com/t/YEeuAYMBBJ

For more information:  http://motzcod.es/post/97662738237/scanning-for-ibeacons-in-ios-8

**iOS 9 Simulator**
Getting location via the simulator doesn't seem to be supported, you will need to test on a device.

**iOS 9 Special Case: Background Updates (for background agents, not background tasks):**

New in iOS 9 allowsBackgroundLocationUpdates must be set if you are running a background agent to track location. I have exposed this on the Geolocator via:

```csharp
var locator = CrossGeolocator.Current;
locator.AllowsBackgroundUpdates = true;
```

The presence of the UIBackgroundModes key with the location value is required for background updates; you use this property to enable and disable the behavior based on your app’s behavior.

#### Windows Phone:

You must set the ID_CAP_LOCATION permission.


#### License
This is a derivative to [Xamarin.Mobile's Geolocator](http://github.com/xamarin/xamarin.mobile) with a cross platform API and other enhancements.
﻿//
//  Copyright 2011-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
