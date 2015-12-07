## ![](http://www.refractored.com/images/plugin_icon_geolocator.png) Geolocator Plugin for Xamarin and Windows

Simple cross platform plugin to get GPS location including heading, speed, and more. Additionally, you can track geolocation changes :)

Ported from [Xamarin.Mobile](http://www.github.com/xamarin/xamarin.mobile) to a cross platform API.

### Setup
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.Geolocator
* Install into your PCL project and Client projects.

**Supports**
* Xamarin.iOS
* Xamarin.iOS (x64 Unified)
* Xamarin.Android
* Windows Phone 8 (Silverlight)
* Windows Phone 8.1 RT
* Windows Store 8.1
* Windows 10 UWP


### API Usage

Call **CrossGeolocator.Current** from any project or PCL to gain access to APIs.

```csharp
var locator = CrossGeolocator.Current;
locator.DesiredAccuracy = 50;

var position = await locator.GetPositionAsync (timeout: 10000);

Console.WriteLine ("Position Status: {0}", position.Timestamp);
Console.WriteLine ("Position Latitude: {0}", position.Latitude);
Console.WriteLine ("Position Longitude: {0}", position.Longitude);
```

### API 

```
/// <summary>
/// Position error event handler
/// </summary>
event EventHandler<PositionErrorEventArgs> PositionError;
```

```
/// <summary>
/// Position changed event handler
/// </summary>
event EventHandler<PositionEventArgs> PositionChanged;
```

```
/// <summary>
/// Desired accuracy in meteres
/// </summary>
double DesiredAccuracy { get; set; }
```

```
/// <summary>
/// Gets if you are listening for location changes
/// </summary>
bool IsListening { get; }
```

```
/// <summary>
/// Gets if device supports heading
/// </summary>
bool SupportsHeading { get; }
```

```
/// <summary>
/// Gets or sets if background updates should be allowed on the geolocator.
/// </summary>
bool AllowsBackgroundUpdates { get; set; }
```

```
/// <summary>
/// Gets or sets if the location updates should be paused automatically (iOS)
/// </summary>
bool PausesLocationUpdatesAutomatically { get; set; }
```

```
/// <summary>
/// Gets if geolocation is available on device
/// </summary>
bool IsGeolocationAvailable { get; }
```

```
/// <summary>
/// Gets if geolocation is enabled on device
/// </summary>
bool IsGeolocationEnabled { get; }
```

```
/// <summary>
/// Gets position async with specified parameters
/// </summary>
/// <param name="timeoutMilliseconds">Timeout in milliseconds to wait, Default Infinite</param>
/// <param name="token">Cancelation token</param>
/// <param name="includeHeading">If you would like to include heading</param>
/// <returns>Position</returns>
Task<Position> GetPositionAsync(int timeoutMilliseconds = Timeout.Infinite, CancellationToken? token = null, bool includeHeading = false);
```

```
/// <summary>
/// Start lisenting for changes
/// </summary>
/// <param name="minTime">Time</param>
/// <param name="minDistance">Distance</param>
/// <param name="includeHeading">Include heading or not</param>
Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false);
```

```
/// <summary>
/// Stop linstening
/// </summary>
Task<bool> StopListeningAsync();
```

### **IMPORTANT**
#### Android:

You must request ACCESS_COARSE_LOCATION & ACCESS_FINE_LOCATION permission

**Android 6.0 Marshmallow**
You will have to ask for Fine&Course Location permissions via the new runtime permissions.
https://blog.xamarin.com/requesting-runtime-permissions-in-android-marshmallow/

See this example: https://github.com/jamesmontemagno/MarshmallowSamples/blob/master/RuntimePermissions/MarshmallowPermission/MainActivity.cs


#### iOS:
In iOS 8 you now have to call either RequestWhenInUseAuthorization or RequestAlwaysAuthorization on the location manager (the plugin does this automatically for you, however, need to add either the concisely named NSLocationWhenInUseUsageDescription or NSLocationAlwaysUsageDescription to your Info.plist. 

You will need to add a new string entry called NSLocationWhenInUseUsageDescription or NSLocationAlwaysUsageDescription. 

Go to your info.plist and under source add one of these flags: http://screencast.com/t/YEeuAYMBBJ

For more information:  http://motzcod.es/post/97662738237/scanning-for-ibeacons-in-ios-8

**iOS 9 Simulator**
Getting location via the simulator doesn't seem to be supported, you will need to test on a device.

**iOS 9 Special Case: Background Updates (for background agents, not background tasks):**

New in iOS 9 allowsBackgroundLocationUpdates must be set if you are running a background agent to track location. I have exposed this on the Geolocator via:

var locator = CrossGeolocator.Current;
locator.AllowsBackgroundUpdates = true;

The presence of the UIBackgroundModes key with the location value is required for background updates; you use this property to enable and disable the behavior based on your app’s behavior.

#### Windows Phone:

You must set the ID_CAP_LOCATION permission.

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

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
