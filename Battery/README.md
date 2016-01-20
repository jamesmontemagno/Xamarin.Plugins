## ![](Common/battery_icon.png) Battery Status Plugin for Xamarin and Windows

Simple cross platform plugin to check batterystatus of mobile device, get remaining percentage, 

### Setup
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.Battery  [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.Battery.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.Battery/)
* Install into your PCL project and Client projects.

Build Status: [![Build status](https://ci.appveyor.com/api/projects/status/78bt2sgh90x3kw5n?svg=true)](https://ci.appveyor.com/project/JamesMontemagno/xamarin-plugins-5759s)

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 6+|
|Xamarin.iOS Unified|Yes|iOS 6+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|Yes|8.1+|
|Windows Store RT|Yes|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|No||


**Windows Store has a blank DLL that always returns 100, Ac, and Full as there is no API for checking battery**


### API Usage

Call **CrossBattery.Current** from any project or PCL to gain access to APIs.


**RemainingChargePercent**
```csharp
/// <summary>
/// Current battery level 0 - 100
/// </summary>
int RemainingChargePercent { get; }
```

**Status**
```csharp
/// <summary>
/// Current status of the battery
/// </summary>
BatteryStatus Status { get; }
```

This returns an enum with the current status of the battery. If charging or not:

```csharp
/// <summary>
/// Current status of battery
/// </summary>
public enum BatteryStatus
{
  /// <summary>
  /// Plugged in and charging
  /// </summary>
  Charging,
  /// <summary>
  /// Battery is being drained currently
  /// </summary>
  Discharging,
  /// <summary>
  /// Battery is full completely
  /// </summary>
  Full,
  /// <summary>
  /// Not charging, but not discharging either
  /// </summary>
  NotCharging,
  /// <summary>
  /// Unknown or other status
  /// </summary>
  Unknown

}
```

Important:
* iOS: only returns Charging, Full, Discharging, and Unknown.
* WP: only returns Charging, Full, Discharging
* WP 8.1 RT: calculats charge/full and starts with unknown status


**PowerSource**
```csharp
/// <summary>
/// Currently how the battery is being charged.
/// </summary>
PowerSource PowerSource { get; }
```

Returns how the phone is being charged

#### Events

You can subscribe to BatteryChanged, which will return BatteryChangedEventArgs with all information you need.
This occurs when plugged, unplugged, or battery change.

```csharp
/// <summary>
/// Event handler when battery changes
/// </summary>
event BatteryChangedEventHandler BatteryChanged;
```

Note: on WP you will only receive this on battery level change.

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
