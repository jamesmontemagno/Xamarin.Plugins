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
* Windows 10 UWP


### API Usage

Call **CrossMedia.Current** from any project or PCL to gain access to APIs.

Before taking photos or videos you should check to see if a camera exists and if photos and videos are supported on the device. There are five properties that you can check:

```csharp
/// <summary>
/// Gets if a camera is available on the device
/// </summary>
bool IsCameraAvailable { get; }

/// <summary>
/// Gets if ability to take photos supported on the device
/// </summary>
bool IsTakePhotoSupported { get; }

/// <summary>
/// Gets if the ability to pick photo is supported on the device
/// </summary>
bool IsPickPhotoSupported { get; }

/// <summary>
/// Gets if ability to take video is supported on the device
/// </summary>
bool IsTakeVideoSupported { get; }

/// <summary>
/// Gets if the ability to pick a video is supported on the device
/// </summary>
bool IsPickVideoSupported { get; }
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

    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
    {
        DisplayAlert("No Camera", ":( No camera available.", "OK");
        return;
    }

    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
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

### Saving Photo/Video to Camera Roll/Gallery
As of Version 2.1.0 ([currently in beta](https://www.nuget.org/packages/Xam.Plugin.Media/2.1.0-beta1)) you can now save a photo or video to the camera roll/gallery. When creating the ```StoreCameraMediaOptions``` or ```StoreVideoMediaOptions``` simply set ```SaveToAlbum``` to true. When your user takes a photo it will still store temporary data, but also if needed make a copy to the public gallery (based on platform). In the MediaFile you will now see a AlbumPath that you can query as well.

Android: When you set SaveToAlbum this will make it so your photos are public in the Pictures/YourDirectory or Movies/YourDirectory. This is the only way Android can detect the photos.


### **IMPORTANT**
**Android:**

You must request `WRITE_EXTERNAL_STORAGE`, `READ_EXTERNAL_STORAGE` & `CAMERA` permissions (these will be done automatically by the Permissions Plugin)

<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />

<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />

<uses-permission android:name="android.permission.CAMERA" />

**Windows Phone 8/8.1 Silverlight:**

You must set the `IC_CAP_ISV_CAMERA` permission.

WP 8/8.1 Silverlight only supports photo, not video.

**Windows Phone 8.1 RT:**

Set `Webcam` permission.

In your App.xaml.cs you MUST place the following code:

```csharp
Plugin.Media.MediaImplementation.OnFilesPicked(args);
```

**Windows Store:**

Set `Webcam` permission.

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### Windows Phone 8.1 RT Photo Capture
This feature was made possible by Daniel Meixner and his great open source project:  https://diycameracaptureui.codeplex.com/

Made possible under Ms-PL license:  https://diycameracaptureui.codeplex.com/license

#### License
This is a derivative to [Xamarin.Mobile's Media](http://github.com/xamarin/xamarin.mobile) with a cross platform API and other enhancements.
﻿//
//  Copyright 2011-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
