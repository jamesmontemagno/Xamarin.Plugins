## Vibrate

Simple but elegant way of performing Vibrate in your Xamarin, Windows, and Xamarin.Forms projects


## Xamarin, Windows, and Xamarin.Forms
This NuGet can be used for all tradition Xamarin and Windows development with or without Xamarin.Forms. There is no requirement of a dependency service as it has a built in Singleton to access the vibrate functionality.


#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xam.Plugins.Vibrate [![NuGet](https://img.shields.io/nuget/v/Xam.Plugins.Vibrate.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugins.Vibrate/)
* Install into your PCL project and Client projects.

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.iOS Unified|Yes|iOS 7+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|Yes|8.1+|
|Windows Store RT|---|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|No||


**ANDROID Specific**
Please ensure you have the VIBRATE persmission enabled (will auto be added now).

```
<uses-permission android:name="android.permission.VIBRATE" />
```

#### API Usage

To gain access to the Vibrate class simply use this method:

```
var v = CrossVibrate.Current;
```

#### Methods

```
 CrossVibrate.Current.Vibration(int milliseconds = 500);
```

Vibrate device for specified amount of time. 500 is the default and will vibrate for 500 milliseconds.

**iOS Specific**
There is no API to vibrate for a specific amount of time, so it will vibrate for the default no matter what.


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
