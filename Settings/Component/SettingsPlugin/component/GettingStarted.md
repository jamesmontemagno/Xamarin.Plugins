# Getting Started with Settings Plugin

### Setup & Usage
* Available on NuGet and Component: https://www.nuget.org/packages/Xam.Plugins.Settings/
* Install into your PCL project and Client projects.
* Open up Helpers/Settings.cs in your PCL for directions on how to use.
* If you are not using a PCL you will find an _SettingsStarted.txt file under properties to get started. Else you can follow this guide:

#### Create a new static class
You will want to create a new `static` class called "Settings" in your shared code project or PCL that will house all of your settings.

```
public static class Settings
{
 // code here...
}
```

#### Gain Access to ISettings
When you want to read/write setting you want to gain access via the ISettings API. I automatically have a singleton setup for you so you just need access:

```
private static ISettings AppSettings
{
  get
  {
    return CrossSettings.Current;
  }
}
```

#### Create your Key and Default Values
Each setting consists of a `const string` key and a default value. I HIGHLY recommend declaring these ahead of time such as:

```
private const string UserNameKey = "username_key";
private static readonly string UserNameDefault = string.Empty;

private const string SomeIntKey = "int_key";
private static readonly int SomeIntDefault = 6251986;
```

#### Create Getters and Setters for your Setting
Now it is time to setup your actual setting that can be accessed from **ANY** project, whether it be a PCL, Shared Code, or a platform specific project. We do this by usign the two methods in the ISettings API: `GetValueOrDefault` and `AddOrUpdateValue`:

```
public static string UserName
{
  get { return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault); }
  set { AppSettings.AddOrUpdateValue<string>(UserNameKey, value); }
}

public static int SomeInt
{
  get { return AppSettings.GetValueOrDefault<int>(SomeIntKey, SomeIntDefault); }
  set { AppSettings.AddOrUpdateValue<int>(SomeIntKey, value); }
}
```

There you have it! You are all done :)


**iOS Specific**
You must enable generic type sharing in the settings. It is on by default on new projects.

![](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/7466bca6-a916-4fd9-9301-3c3403d3a6ad/00000097.png)

**Supports**
* Xamarin.iOS
* Xamarin.iOS (Unified)
* Xamarin.Android
* Windows Phone 8 (Silverlight)
* Windows Pone 8.1 RT
* Windows Store 8.0+