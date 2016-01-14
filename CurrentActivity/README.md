## ![](plugin_current_activity.png) Current Activity Plugin for Xamarin.Android

This plugin gives developers and library creators easy access to an Android Application’s current Activity that is being displayed.

Want to read about the creation, checkout my [in-depth blog post](http://motzcod.es/post/133609925342/access-the-current-android-activity-from-anywhere).


### Setup
* Available on NuGet: http://www.nuget.org/packages/Plugin.CurrentActivity [![NuGet](https://img.shields.io/nuget/v/Plugin.CurrentActivity.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.CurrentActivity/)
* Install into your Xamarin.Android Client project.

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|---|iOS 6+|
|Xamarin.iOS Unified|---|iOS 6+|
|Xamarin.Android|Yes|API 14+|
|Windows Phone Silverlight|---|8.0+|
|Windows Phone RT|---|8.1+|
|Windows Store RT|---|8.1+|
|Windows 10 UWP|---|10+|
|Xamarin.Mac|---||


### API Usage

Call **CrossCurrentActivity.Current** from any project or PCL to gain access to APIs.


**Activity**
```
/// <summary>
/// The Current Activity
/// </summary>
Activity Activity { get; set; }
```

That’s it! Well not really:

**Application Setup**
When you install this plugin a **MainApplication.cs** is installed into your android project. If you already have an Application class then you should copy over the important bits that can be found in the Readme file that was also installed in the PluginsHelp folder


**Library Creators**
Simply set this nuget as a dependency of your project to gain access to the current activity. This can be achieved by setting the following in your nuspec:

```
<dependencies>
  <group targetFramework="MonoAndroid10">
    <dependency id="Plugin.CurrentActivity" version="1.0.0"/>
  </group>
</dependencies>
```

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
