## ![](Common/circle_image_icon.png)Circle Image Control Plugin for Xamarin.Forms

Simple but elegant way of display circle images in your Xamarin.Forms projects

#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xam.Plugins.Forms.ImageCircle [![NuGet](https://img.shields.io/nuget/v/Xam.Plugins.Forms.ImageCircle.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugins.Forms.ImageCircle/)
* Install into your PCL project and Client projects.


In your iOS, Android, and Windows projects call:

```
Xamarin.Forms.Init();//platform specific init
ImageCircleRenderer.Init();
```

You must do this AFTER you call Xamarin.Forms.Init();

#### Usage
Instead of using an Image simply use a CircleImage instead!

You **MUST** set the width & height requests to the same value and you will want to use AspectFill. Here is a sample:
```
new CircleImage
{
  BorderColor = Color.White,
  BorderThickness = 3,
  HeightRequest = 150,
  WidthRequest = 150,
  Aspect = Aspect.AspectFill,
  HorizontalOptions = LayoutOptions.Center,
  Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/55/Tamarin_portrait.JPG"))
}
```

**XAML:**

First add the xmlns namespace:
```xml
xmlns:controls="clr-namespace:ImageCircle.Forms.Plugin.Abstractions;assembly=ImageCircle.Forms.Plugin.Abstractions"
```

Then add the xaml:

```xml
<controls:CircleImage Source="{Binding Image}">
  <controls:CircleImage.WidthRequest>
    <OnPlatform x:TypeArguments="x:Double"
      iOS="55"
      Android="55"
      WinPhone="75"/>
   </controls:CircleImage.WidthRequest>
<controls:CircleImage.HeightRequest>
    <OnPlatform x:TypeArguments="x:Double">
      iOS="55"
      Android="55"
      WinPhone="75"/>
   </controls:CircleImage.HeightRequest>
</controls:CircleImage>
```


**Bindable Properties**

You are able to set the ```BorderColor``` to a Forms.Color to display a border around your image and also ```BorderThickness``` for how thick you want it. 

You can also set ```FillColor``` to the Forms.Color to fill the circle. DO NOT set ```BackgroundColor``` as that will be the square the entire image takes up.

These are supported in iOS, Android, WinRT, and UWP (not on Windows Phone 8 Silverlight).

**Final Builds**
For linking you may need to add:

Android:

ImageCircle.Forms.Plugin.Abstractions;ImageCircle.Forms.Plugin.Android;

iOS:

--linkskip=ImageCircle.Forms.Plugin.iOS --linkskip=ImageCircle.Forms.Plugin.Abstractions

**Supports**
* Xamarin.iOS
* Xamarin.iOS (x64 Unified)
* Xamarin.Android
* Windows Phone 8 (Silverlight)
* Windows Phone 8.1 RT
* Windows Store 8.1
* Windows 10 UWP


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
