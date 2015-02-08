
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
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Devices;
using Microsoft.Phone.Tasks;

using Media.Plugin.Abstractions;


namespace Media.Plugin
{
  /// <summary>
  /// Implementation for Media
  /// </summary>
  public class MediaImplementation : IMedia
  {
    public MediaImplementation()
    {

      this.photoChooser.Completed += OnPhotoChosen;
      this.photoChooser.ShowCamera = false;

      this.cameraCapture.Completed += OnPhotoChosen;

      IsCameraAvailable = Camera.IsCameraTypeSupported(CameraType.Primary) || Camera.IsCameraTypeSupported(CameraType.FrontFacing);
    }

    public bool IsCameraAvailable
    {
      get;
      private set;
    }

    public bool PhotosSupported
    {
      get { return true; }
    }

    public bool VideosSupported
    {
      get { return false; }
    }

    public Task<MediaFile> PickPhotoAsync()
    {
      var ntcs = new TaskCompletionSource<MediaFile>();
      if (Interlocked.CompareExchange(ref this.completionSource, ntcs, null) != null)
        throw new InvalidOperationException("Only one operation can be active at at time");

      this.photoChooser.Show();

      return ntcs.Task;
    }

    public Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
    {
      options.VerifyOptions();

      var ntcs = new TaskCompletionSource<MediaFile>(options);
      if (Interlocked.CompareExchange(ref this.completionSource, ntcs, null) != null)
        throw new InvalidOperationException("Only one operation can be active at a time");

      this.cameraCapture.Show();

      return ntcs.Task;
    }

    public Task<MediaFile> PickVideoAsync()
    {
      throw new NotSupportedException();
    }

    public Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
    {
      throw new NotSupportedException();
    }

    private readonly CameraCaptureTask cameraCapture = new CameraCaptureTask();
    private readonly PhotoChooserTask photoChooser = new PhotoChooserTask();
    private TaskCompletionSource<MediaFile> completionSource;

    private void OnPhotoChosen(object sender, PhotoResult photoResult)
    {
      var tcs = Interlocked.Exchange(ref this.completionSource, null);

      if (photoResult.TaskResult == TaskResult.Cancel)
      {
        tcs.SetCanceled();
        return;
      }

      string path = photoResult.OriginalFileName;

      long pos = photoResult.ChosenPhoto.Position;
      var options = tcs.Task.AsyncState as StoreCameraMediaOptions;
      using (var store = IsolatedStorageFile.GetUserStoreForApplication())
      {
        path = options.GetUniqueFilepath((options == null) ? "temp" : null, p => store.FileExists(p));

        string dir = Path.GetDirectoryName(path);
        if (!String.IsNullOrWhiteSpace(dir))
          store.CreateDirectory(dir);

        using (var fs = store.CreateFile(path))
        {
          byte[] buffer = new byte[20480];
          int len;
          while ((len = photoResult.ChosenPhoto.Read(buffer, 0, buffer.Length)) > 0)
            fs.Write(buffer, 0, len);

          fs.Flush(flushToDisk: true);
        }
      }

      Action<bool> dispose = null;
      if (options == null)
      {
        dispose = d =>
        {
          using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            store.DeleteFile(path);
        };
      }

      switch (photoResult.TaskResult)
      {
        case TaskResult.OK:
          photoResult.ChosenPhoto.Position = pos;
          tcs.SetResult(new MediaFile(path, () => photoResult.ChosenPhoto, dispose: dispose));
          break;

        case TaskResult.None:
          photoResult.ChosenPhoto.Dispose();
          if (photoResult.Error != null)
            tcs.SetException(photoResult.Error);

          break;
      }
    }
  }
}