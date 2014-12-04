## Vibrate

Simple but elegant way of performing Vibrate in your Xamarin.Forms projects

### Xamarin.Forms
Currently implementation is only for Xamarin.Forms, but I will publish a traditional version soon.

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

### API Usage

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


### Traditional Xamarin
Coming Soon


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
