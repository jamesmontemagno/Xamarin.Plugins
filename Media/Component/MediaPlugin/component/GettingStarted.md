# Getting Started with Media Plugin



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
**Android:**

You must request `WRITE_EXTERNAL_STORAGE` permission

**Windows Phone 8/8.1 Silverlight:**

You must set the `IC_CAP_ISV_CAMERA` permission.

WP 8/8.1 Silverlight only supports photo, not video.

**Windows Phone 8.1 RT:**

Set `Webcam` permission.

In your App.xaml.cs you MUST place the following code:

```csharp
Media.Plugin.MediaImplementation.OnFilesPicked(args);
```

**Windows Store:**

Set `Webcam` permission.
