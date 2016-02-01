Media Plugin for Xamarin & Windows

Changelog:
[2.2.0]
-Android: No longer require camera permission
-Android: Use latest permission plugin for External Storage Permissions
-Android: Try to show front facing camera when parameter is set

-2.1.2
--Add new "SaveToAlbum" bool to save the photo to public gallery
--Added new "AlbumPath", which is set when you use the "SaveToAlbum" settings
--->Supports iOS, Android, WinRT, and UWP
--Add readme.txt
--Update to latest permissions plugin

-2.0.1
--Breaking changes: New namespace - Plugin.Media
--Automatically Add Android Permissions
--Request Android Permissions on Marshmallow
----Uses new Permissions Plugin : https://www.nuget.org/packages/Plugin.Permissions/
---UWP Support

Learn More:
http://www.github.com/jamesmontemagno/Xamarn.Plugins
http://www.xamarin.com/plugins

Created by James Montemagno:
http://twitter.com/jamesmontemagno
http://www.motzcod.es


Documentation:

Call **CrossMedia.Current** from any project or PCL to gain access to APIs.

Before taking photos or videos you should check to see if a camera exists and if photos and videos are supported on the device. There are five properties that you can check:

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

### Photos
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

### Videos
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


### Usage
Via a Xamarin.Forms project with a Button and Image to take a photo:

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


### Saving Photo/Video to Camera Roll/Gallery
As of Version 2.1.0 you can now save a photo or video to the camera roll/gallery. 
When creating the ```StoreCameraMediaOptions``` or ```StoreVideoMediaOptions``` simply set ```SaveToAlbum``` to true. 
When your user takes a photo it will still store temporary data, but also if needed make a copy to the public gallery (based on platform).
 In the MediaFile you will now see a AlbumPath that you can query as well.

Android: When you set SaveToAlbum this will make it so your photos are public in the Pictures/YourDirectory or Movies/YourDirectory.
This is the only way Android can detect the photos.

Windows Phone 8 Silverlight: Photos are automatically saved to camera roll no mater what, this is a limitation of the API.


### Additional Setup
Permissions Requirements:
**Android:**
The `WRITE_EXTERNAL_STORAGE`, `READ_EXTERNAL_STORAGE` permissions are required, but the library will automatically add this for you. Additionally, if your users are running Marshmallow the Plugin will automatically prompt them for runtime permissions.

Additionally, the following has been added for you:
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]


**iOS** 

The library will automatically ask for permission when taking photos/videos or access the libraries.

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