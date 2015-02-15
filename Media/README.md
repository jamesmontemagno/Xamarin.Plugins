## ![](http://www.refractored.com/images/plugin_icon_media.png) Media Plugin for Xamarin and Windows

Simple cross platform plugin to take photos and video or pick them from a gallery from shared code.

Ported from [Xamarin.Mobile](http://www.github.com/xamarin/xamarin.mobile) to a cross platform API.

### Setup
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.Media
* Install into your PCL project and Client projects.

**Supports**
* Xamarin.iOS
* Xamarin.iOS (x64 Unified)
* Xamarin.Android
* Windows Phone 8/8.1 (Silverlight)
* Windows Phone 8.1 RT
* Windows Store 8.1


### API Usage

Call **CrossMedia.Current** from any project or PCL to gain access to APIs.

Before taking photos or videos you should check to see if a camera exists and if photos and videos are supported on the device. There are three properties that you can check:

```csharp
/// <summary>
/// Gets if a camera is available on the device
/// </summary>
bool IsCameraAvailable { get; }
    
/// <summary>
/// Gets if Photos are supported on the device
/// </summary>
bool PhotosSupported { get; }
    
/// <summary>
/// Gets if Videos are supported on the device
/// </summary>
bool VideosSupported { get; }
```

### Photos
```csharp
/// <summary>
/// Picks a photo from the default gallery
/// </summary>
/// <returns>Media file or null if canceled</returns>
Task<MediaFile> PickPhotoAsync();

/// <summary>
/// Take a photo async with specified options
/// </summary>
/// <param name="options">Camera Media Options</param>
/// <returns>Media file of photo or null if canceled</returns>
Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options);
```

### Videos
```csharp
/// <summary>
/// Picks a video from the default gallery
/// </summary>
/// <returns>Media file of video or null if canceled</returns>
Task<MediaFile> PickVideoAsync();

/// <summary>
/// Take a video with specified options
/// </summary>
/// <param name="options">Video Media Options</param>
/// <returns>Media file of new video or null if canceled</returns>
Task<MediaFile> TakeVideoAsync(StoreVideoOptions options);
```

### Usage
Via a Xamarin.Forms project with a Button and Image to take a photo:

```csharp
      takePhoto.Clicked += async (sender, args) =>
        {

          if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.PhotosSupported)
          {
            DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            return;
          }

          var file = await CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
            {

              Directory = "Sample",
              Name = "test.jpg"
            });

          if (file == null)
            return;

          DisplayAlert("File Location", file.Path, "OK");

          image.Source = ImageSource.FromStream(() =>
          {
            var stream = file.GetStream();
            file.Dispose();
            return stream;
          }); 
        };
```


### **IMPORTANT**
Android:

You must request `WRITE_EXTERNAL_STORAGE` permission

Windows Phone 8/8.1 Silverlight:

You must set the `IC_CAP_ISV_CAMERA` permission.

In your App.xaml.cs you MUST place the following code:

```csharp
Media.Plugin.MediaImplementation.OnFilesPicked(args);
```

Windows Phone 8.1 RT:

Set `Webcam` permission.

In your App.xaml.cs you MUST place the following code:

```csharp
Media.Plugin.MediaImplementation.OnFilesPicked(args);
```

Windows Store:

Set `Webcam` permission.

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### Windows Phone 8.1 RT Photo Capture
This feature was made possible by Daniel Meixner and his great open source project:  https://diycameracaptureui.codeplex.com/

Made possible under Ms-PL license:  https://diycameracaptureui.codeplex.com/license

#### License
Licensed under main repo license
