## ![](Common/DeviceInfoPlugin/component/DeviceInfoPlugin_128x128.png) Device Info Plugin for Xamarin

Simple way of getting common device information.

### Setup
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.DeviceInfo [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.DeviceInfo.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.DeviceInfo/)
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


### API Usage

Call **CrossDeviceInfo.Current** from any project or PCL to gain access to APIs.

**GenerateAppId**
Used to generate a unique Id for your app.

```csharp
/// <summary>
/// Generates a an AppId optionally using the PhoneId a prefix and a suffix and a Guid to ensure uniqueness
/// 
/// The AppId format is as follows {prefix}guid{phoneid}{suffix}, where parts in {} are optional.
/// </summary>
/// <param name="usingPhoneId">Setting this to true adds the device specific id to the AppId (remember to give the app the correct permissions)</param>
/// <param name="prefix">Sets the prefix of the AppId</param>
/// <param name="suffix">Sets the suffix of the AppId</param>
/// <returns></returns>
string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null);
```

**Id**
```csharp
/// <summary>
/// This is the device specific Id (remember the correct permissions in your app to use this)
/// </summary>
string Id { get; }
```
Important:

Windows Phone:
Permissions to add:
ID_CAP_IDENTITY_DEVICE

**Device Model**
```csharp
/// <summary>
/// Get the model of the device
/// </summary>
string Model { get; }
```


**Version**
```csharp
/// <summary>
/// Get the version of the Operating System
/// </summary>
string Version { get; }
```

Returns the specific version number of the OS such as:
* iOS: 8.1
* Android: 4.4.4
* Windows Phone: 8.10.14219.0
* WinRT: always 8.1 until there is a work around

**Platform**
```csharp
/// <summary>
/// Get the platform of the device
/// </summary>
Platform Platform { get; }
```

Returns the Platform Enum of:
```csharp
public enum Platform
{
  Android,
  iOS,
  WindowsPhone,
  Windows
}
```

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Dirived from: [@Cheesebaron](http://www.github.com/cheesebaron)
//---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------
