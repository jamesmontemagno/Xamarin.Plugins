## ![](Common/circle_image_icon.png)Circle Image Control Plugin for Xamarin.Forms

Simple but elegant way of display circle images in your Xamarin.Forms projects

#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xam.Plugins.Forms.ImageCircle
* Install into your PCL project and Client projects.

In your iOS, Android, and Windows Phone projects call:

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
    <OnPlatform x:TypeArguments="x:Double">
      <OnPlatform.iOS>55</OnPlatform.iOS>
      <OnPlatform.Android>55 </OnPlatform.Android>
      <OnPlatform.WinPhone>75</OnPlatform.WinPhone>
    </OnPlatform>
   </controls:CircleImage.WidthRequest>
<controls:CircleImage.HeightRequest>
    <OnPlatform x:TypeArguments="x:Double">
      <OnPlatform.iOS>55</OnPlatform.iOS>
      <OnPlatform.Android>55</OnPlatform.Android>
      <OnPlatform.WinPhone>75</OnPlatform.WinPhone>
    </OnPlatform>
   </controls:CircleImage.HeightRequest>
</controls:CircleImage>
```


**Bindable Properties**

You are able to set the ```BorderColor``` to a Forms.Color to display a border around your image and also ```BorderThickness``` for how thick you want it. This is supported in iOS & Android only.


**Final Builds**
For linking you may need to add:

Android:

ImageCircle.Forms.Plugin.Abstractions;ImageCircle.Forms.Plugin.Android;

iOS:

--linkskip=ImageCircle.Forms.Plugin.iOS --linkskip=ImageCircle.Forms.Plugin.Abstractions

Android:


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
