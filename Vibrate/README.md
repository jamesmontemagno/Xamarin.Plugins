## Vibrate

Simple but elegant way of performing Vibrate in your Xamarin, Windows, and Xamarin.Forms projects


## Xamarin, Windows, and Xamarin.Forms
This NuGet can be used for all tradition Xamarin and Windows development with or without Xamarin.Forms. There is no requirement of a dependency service as it has a built in Singleton to access the vibrate functionality.


#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xam.Plugins.Vibrate
* Install into your PCL project and Client projects.

**Supports**
* Xamarin.iOS
* Xamarin.iOS (x64 Unified)
* Xamarin.Android
* Windows Phone 8 (Silverlight)
* Windows Phone 8.1 RT
* Windows Store 8.1 (Vibrate not supported on these devices, just a blank DLL installed)

**ANDROID Specific**
Please ensure you have the VIBRATE persmission enabled:

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
