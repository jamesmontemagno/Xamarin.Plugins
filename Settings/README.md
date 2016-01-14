## ![](https://raw.githubusercontent.com/jamesmontemagno/Xamarin.Plugins/master/Settings/Common/pcl_settings_icon_small.png) Settings Plugin for Xamarin And Windows

Create and access settings from shared code across all of your mobile apps!

### Uses the native settings management
* Android: SharedPreferences
* iOS: NSUserDefaults
* Windows Phone: IsolatedStorageSettings
* Windows RT / UWP: ApplicationDataContainer

### Setup & Usage
* Available on NuGet: https://www.nuget.org/packages/Xam.Plugins.Settings/ [![NuGet](https://img.shields.io/nuget/v/Xam.Plugins.Settings.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugins.Settings/)
* Install into your PCL project and Client projects.
* Open up Helpers/Settings.cs in your PCL for directions on how to use.
* If you are not using a PCL you will find an _SettingsStarted.txt file under properties to get started. Else you can follow this guide:


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

#### Create a new static class
You will want to create a new `static` class called "Settings" in your shared code project or PCL that will house all of your settings.

```csharp
public static class Settings
{
 // code here...
}
```

#### Gain Access to ISettings
When you want to read/write setting you want to gain access via the ISettings API. I automatically have a singleton setup for you so you just need access:

```csharp
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

```csharp
private const string UserNameKey = "username_key";
private static readonly string UserNameDefault = string.Empty;

private const string SomeIntKey = "int_key";
private static readonly int SomeIntDefault = 6251986;
```

#### Create Getters and Setters for your Setting
Now it is time to setup your actual setting that can be accessed from **ANY** project, whether it be a PCL, Shared Code, or a platform specific project. We do this by usign the two methods in the ISettings API: `GetValueOrDefault` and `AddOrUpdateValue`:

```csharp
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


### Data Types Supported
* Boolean
* Int32 
* Int64 
* String 
* Single(float) 
* Guid 
* Double 
* Decimal 
* DateTime

### NuGet Creation & Packaging

I created this NuGet PCL package by creating an interface that I would implement on each platform. This lives inside of my .Abstractions.dll that will be installed on each platform. I then create a base PCL project that has a Settings.cs file with an internal IoC to new up a CrossSettings class. The key here is that this file is linked to all of my platform specific projects to new up the correct version.

You can find my nuspec here: https://github.com/jamesmontemagno/Xam.PCL.Plugins/blob/master/Settings/Common/Xam.Plugins.Settings.nuspec


### DataBinding in Xamarin.Forms
I got a really great question over at StackOverflow as to how to data bind this puppy up for views and there are two approaches. I prefer the first one as it keeps everything all static, but you can of course do it via a singleton too:

Given this Settings Class:
```csharp
public static class Settings 
{

    private static ISettings AppSettings
    {
        get
        {
            return CrossSettings.Current;
        }
    }

    #region Setting Constants

    const string CountKey = "count";
    private static readonly int CountDefault = 0;



    #endregion



    public static int Count
    {
        get { return AppSettings.GetValueOrDefault<int>(CountKey, CountDefault); }
        set { AppSettings.AddOrUpdateValue<int>(CountKey, value); }
    }
}
```

Approach 1: Essentially you need to create a view model with a public property that you wish to data bind to and then call into settings from there and raise a property changed notification if the value changed. Your Settings.cs can stay the same but you will need to create the viewmodel such as:
```csharp
public class MyViewModel : INotifyPropertyChanged
{

    public int Count
    {
        get { return Settings.Count; }
        set
        {
            if (Settings.Count == value)
                return;

            Settings.Count = value;
            OnPropertyChanged();
        }
            
    }

    private Command increase;
    public Command IncreaseCommand
    {
        get 
        { 
            return increase ?? (increase = new Command(() =>Count++));
        }
    }

    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName]string name = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;
        changed(this, new PropertyChangedEventArgs(name));
    }

    #endregion


}
```
Then you XAML will look like this inside your Content page:
```xaml
<StackLayout Padding="25">
 <Button Text="Increase" Command="{Binding IncreaseCommand}"/>
 <Label Text="{Binding Count, StringFormat='The count is {0:F0}'}"/>
</StackLayout>
```

Make sure you set the BindingContext in the xaml.cs of the page:
```csharp
public partial class MyPage : ContentPage
{
    public MyPage()
    {
        InitializeComponent();
        BindingContext = new MyViewModel();
    }
}
```

This actually isn't too much code to actually implement as your ViewModel would have a BaseViewModel that implements INotifyProprety changed, so really you are just adding in 

```csharp
public int Count
{
    get { return Settings.Count; }
    set
    {
        if (Settings.Count == value)
            return;

        Settings.Count = value;
        OnPropertyChanged();
    }
}
```


### Approach 2: More magical way

However, using the powers of C# and knowing how Databinding works you could first create a BaseViewModel that everything will use:

```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    public Settings Settings
    {
        get { return Settings.Current; }
    }


    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName]string name = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;
        changed(this, new PropertyChangedEventArgs(name));
    }

    #endregion
}
```

Notice my reference to **Settings.Current**, we will need to implement that now as a singleton, but we will use our BaseViewModel so we don't have to re-implement INotifyPropertyChanged:

```cshapr
public class Settings : BaseViewModel
{
    static ISettings AppSettings
    {
        get
        {
            return CrossSettings.Current;
        }
    }

    static Settings settings;
    public static Settings Current
    {
        get { return settings ?? (settings = new Settings()); }
    }

    #region Setting Constants

    const string CountKey = "count";
    static readonly int CountDefault = 0;

    #endregion

    public int Count
    {
        get
        { 
            return AppSettings.GetValueOrDefault<int>(CountKey, CountDefault); 
        }
        set
        { 
            if (AppSettings.AddOrUpdateValue<int>(CountKey, value))
                OnPropertyChanged();

        }
    }
}
```

Now of course we will still want to create a unique ViewModel that our XAML view will bind to:
```csharp
public class MyViewModel : BaseViewModel
{
    private Command increase;
    public Command IncreaseCommand
    {
        get 
        { 
            return increase ?? (increase = new Command(() =>Settings.Count++));
        }
    }
}
```

Notice that we are now inheriting from BaseViewModel, which means our command can actually just increment Settings.HotTimeCount! But now we must adjust our Xaml just a bit as to what we are actually data binding to for our label:

```xaml
<StackLayout Padding="25">
 <Button Text="Increase" Command="{Binding IncreaseCommand}"/>
 <Label BindingContext="{Binding Settings}" Text="{Binding Count, StringFormat='The count is {0:F0}'}"/>
</StackLayout>
```
Notice I am setting the BindingContext to our Settings, which is in our BaseViewModel for the Label, this must be done because that is where it is located now. And there you have it.


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Main license of Repo
