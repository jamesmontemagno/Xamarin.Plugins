## ![](http://www.refractored.com/images/plugin_icon_geolocator.png) Geolocator Plugin for Xamarin and Windows

Simple cross platform plugin to get GPS location including heading, speed, and more.

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


### API Usage

Call **CrossGeolocator.Current** from any project or PCL to gain access to APIs.

```
var locator = CrossGeolocator.Current;
locator.DesiredAccuracy = 50;

var position = await locator.GetPositionAsync (timeout: 10000);

Console.WriteLine ("Position Status: {0}", position.Timestamp);
Console.WriteLine ("Position Latitude: {0}", position.Latitude);
Console.WriteLine ("Position Longitude: {0}", position.Longitude);
```

### **IMPORTANT**
Android:

You must request ACCESS_COARSE_LOCATION & ACCESS_FINE_LOCATION permission

iOS:

iOS:
In iOS 8 you now have to call either RequestWhenInUseAuthorization or RequestAlwaysAuthorization on the location manager. Additionally you need to add either the concisely named NSLocationWhenInUseUsageDescription or NSLocationAlwaysUsageDescription to your Info.plist. 
See:  http://motzcod.es/post/97662738237/scanning-for-ibeacons-in-ios-8

You will need to add a new string entry called NSLocationWhenInUseUsageDescription or NSLocationAlwaysUsageDescription.

iOS Background Updates (for background agents, not background tasks):

New in iOS 9 allowsBackgroundLocationUpdates must be set if you are running a background agent to track location. I have exposed this on the Geolocator via:

var locator = CrossGeolocator.Current;
locator.AllowsBackgroundUpdates = true;

The presence of the UIBackgroundModes key with the location value is required for background updates; you use this property to enable and disable the behavior based on your app’s behavior.

Windows Phone:

You must set the ID_CAP_LOCATION permission.

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
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
