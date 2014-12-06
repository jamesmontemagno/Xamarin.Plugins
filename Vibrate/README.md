## Vibrate

Simple but elegant way of performing Vibrate in your Xamarin, Windows, and Xamarin.Forms projects

## Xamarin.Forms
There are 2 current implementations, a strict Xamarin.Forms Plugin using the Dependency service and a more traditional approach without the dependency service.

#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xamarin.Plugins.Forms.Vibrate
* Install into your PCL project and Client projects.

In your iOS, Android, and Windows Phone projects please call:

```
Xamarin.Forms.Init();//platform specific init
Vibrate.Init();
```

You must do this AFTER you call Xamarin.Forms.Init();


**ANDROID Specific**
Please ensure you have the VIBRATE persmission enabled:

```
<uses-permission android:name="android.permission.VIBRATE" />
```

#### API Usage

To gain access to the Vibrate class simply use the dependency service:

```
var v = DependencyService.Get<IVibrate>();
```

#### Methods

```
Vibrate(int milliseconds = 0);
```

Vibrate device for specified amount of time. 0 is the default and will vibrate for 500 milliseconds.

**iOS Specific**
There is no API to vibrate for a specific amount of time, so it will vibrate for the default no matter what.



## Traditional Xamarin
This NuGet can be used for all tradition Xamarin and Windows development with out without Xamarin.Forms. There is no requirement of a dependency service as it has a built in Singleton to access the vibrate functionality.


#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xamarin.Plugins.Forms
* Install into your PCL project and Client projects.

**Supports**
* Xamarin.iOS
* Xamarin.iOS (x64 Unified)
* Xamarin.Android
* Windows Phone 8 (Silverlight)
* Windows Phone 8.1 RT
* 
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
 CrossVibrate.Current.Vibrate(int milliseconds = 0);
```

Vibrate device for specified amount of time. 0 is the default and will vibrate for 500 milliseconds.

**iOS Specific**
There is no API to vibrate for a specific amount of time, so it will vibrate for the default no matter what.


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
