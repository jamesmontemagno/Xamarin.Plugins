//
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;

using Media.Plugin.Abstractions;


namespace Media.Plugin
{
  /// <summary>
  /// Implementation for Media
  /// </summary>
  public class MediaImplementation : IMedia
  {
    private static readonly IEnumerable<string> SupportedVideoFileTypes = new List<string> { ".mp4", ".wmv", ".avi" };
    private static readonly IEnumerable<string> SupportedImageFileTypes = new List<string> { ".jpeg", ".jpg", ".png", ".gif", ".bmp" };
    /// <summary>
    /// Implementation
    /// </summary>
    public MediaImplementation()
    {
     
      this.watcher = DeviceInformation.CreateWatcher(DeviceClass.VideoCapture);
      this.watcher.Added += OnDeviceAdded;
      this.watcher.Updated += OnDeviceUpdated;
      this.watcher.Removed += OnDeviceRemoved;
      this.watcher.Start();

      this.init = DeviceInformation.FindAllAsync(DeviceClass.VideoCapture).AsTask()
                                   .ContinueWith(t =>
                                   {
                                     if (t.IsFaulted || t.IsCanceled)
                                       return;

                                     lock (this.devices)
                                     {
                                       foreach (DeviceInformation device in t.Result)
                                       {
                                         if (device.IsEnabled)
                                           this.devices.Add(device.Id);
                                       }

                                       this.isCameraAvailable = (this.devices.Count > 0);
                                     }

                                     this.init = null;
                                   });
    }
    /// <inheritdoc/>
    public bool IsCameraAvailable
    {
      get
      {
        if (this.init != null)
          this.init.Wait();

        return this.isCameraAvailable;
      }
    }
    /// <inheritdoc/>
    public bool IsTakePhotoSupported
    {
      get { return true; }
    }
    /// <inheritdoc/>
    public bool IsPickPhotoSupported
    {
      get { return true; }
    }
    /// <inheritdoc/>
    public bool IsTakeVideoSupported
    {
      get { return true; }
    }
    /// <inheritdoc/>
    public bool IsPickVideoSupported
    {
      get { return true; }
    }

    /// <summary>
    /// Take a photo async with specified options
    /// </summary>
    /// <param name="options">Camera Media Options</param>
    /// <returns>Media file of photo or null if canceled</returns>
    public async Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
    {
      if (!IsCameraAvailable)
        throw new NotSupportedException();

      options.VerifyOptions();

      var capture = new CameraCaptureUI();
      capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
      capture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
     
      var result = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);
      if (result == null)
        return null;

      StorageFolder folder = ApplicationData.Current.LocalFolder;

      string path = options.GetFilePath(folder.Path);
      var directoryFull = Path.GetDirectoryName(path);
      var newFolder = directoryFull.Replace(folder.Path, string.Empty);
      if (!string.IsNullOrWhiteSpace(newFolder))
        await folder.CreateFolderAsync(newFolder, CreationCollisionOption.OpenIfExists);

      folder = await StorageFolder.GetFolderFromPathAsync(directoryFull);

      string filename = Path.GetFileName(path);

      var file = await result.CopyAsync(folder, filename, NameCollisionOption.GenerateUniqueName).AsTask();
      return new MediaFile(file.Path, () => file.OpenStreamForReadAsync().Result);
    }

    /// <summary>
    /// Picks a photo from the default gallery
    /// </summary>
    /// <returns>Media file or null if canceled</returns>
    public async Task<MediaFile> PickPhotoAsync()
    {
      var picker = new FileOpenPicker();
      picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
      picker.ViewMode = PickerViewMode.Thumbnail;
      foreach (var filter in SupportedImageFileTypes)
        picker.FileTypeFilter.Add(filter);

      var result = await picker.PickSingleFileAsync();
      if (result == null)
        return null;

      return new MediaFile(result.Path, () => result.OpenStreamForReadAsync().Result);
    }

    /// <summary>
    /// Take a video with specified options
    /// </summary>
    /// <param name="options">Video Media Options</param>
    /// <returns>Media file of new video or null if canceled</returns>
    public async Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
    {
      if (!IsCameraAvailable)
        throw new NotSupportedException();

      options.VerifyOptions();

      var capture = new CameraCaptureUI();
      capture.VideoSettings.MaxResolution = GetResolutionFromQuality(options.Quality);
      capture.VideoSettings.MaxDurationInSeconds = (float)options.DesiredLength.TotalSeconds;
      capture.VideoSettings.Format = CameraCaptureUIVideoFormat.Mp4;

      var result = await capture.CaptureFileAsync(CameraCaptureUIMode.Video);
      if (result == null)
        return null;

      return new MediaFile(result.Path, () => result.OpenStreamForReadAsync().Result);
    }

    /// <summary>
    /// Picks a video from the default gallery
    /// </summary>
    /// <returns>Media file of video or null if canceled</returns>
    public async Task<MediaFile> PickVideoAsync()
    {
      var picker = new FileOpenPicker();
      picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
      picker.ViewMode = PickerViewMode.Thumbnail;
      foreach (var filter in SupportedVideoFileTypes)
        picker.FileTypeFilter.Add(filter);

      var result = await picker.PickSingleFileAsync();
      if (result == null)
        return null;

      return new MediaFile(result.Path, () => result.OpenStreamForReadAsync().Result);
    }

    private Task init;
    private readonly HashSet<string> devices = new HashSet<string>();
    private readonly DeviceWatcher watcher;
    private bool isCameraAvailable;


    private CameraCaptureUIMaxVideoResolution GetResolutionFromQuality(VideoQuality quality)
    {
      switch (quality)
      {
        case VideoQuality.High:
          return CameraCaptureUIMaxVideoResolution.HighestAvailable;
        case VideoQuality.Medium:
          return CameraCaptureUIMaxVideoResolution.StandardDefinition;
        case VideoQuality.Low:
          return CameraCaptureUIMaxVideoResolution.LowDefinition;
        default:
          return CameraCaptureUIMaxVideoResolution.HighestAvailable;
      }
    }

    private void OnDeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate update)
    {
      object value;
      if (!update.Properties.TryGetValue("System.Devices.InterfaceEnabled", out value))
        return;

      lock (this.devices)
      {
        if ((bool)value)
          this.devices.Add(update.Id);
        else
          this.devices.Remove(update.Id);

        this.isCameraAvailable = this.devices.Count > 0;
      }
    }

    private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate update)
    {
      lock (this.devices)
      {
        this.devices.Remove(update.Id);
        if (this.devices.Count == 0)
          this.isCameraAvailable = false;
      }
    }

    private void OnDeviceAdded(DeviceWatcher sender, DeviceInformation device)
    {
      if (!device.IsEnabled)
        return;

      lock (this.devices)
      {
        this.devices.Add(device.Id);
        this.isCameraAvailable = true;
      }
    }
  }
}