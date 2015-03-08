# Media Plugin Details

 Simple cross platform plugin to take photos and video or pick them from a gallery from shared code.



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