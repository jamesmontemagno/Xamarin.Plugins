
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

using Plugin.Media.Abstractions;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.Storage;
using Windows.ApplicationModel.Activation;


namespace Plugin.Media
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

            this.photoChooser.Completed += OnPhotoChosen;
            this.photoChooser.ShowCamera = false;

            this.cameraCapture.Completed += OnPhotoChosen;

            try
            {
                IsCameraAvailable = Camera.IsCameraTypeSupported(CameraType.Primary) || Camera.IsCameraTypeSupported(CameraType.FrontFacing);
            }
            catch
            {
                Console.WriteLine("You must set the ID_CAP_ISV_CAMERA permission.");
            }
        }

        ///<inheritdoc/>
        public Task<bool> Initialize()
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public bool IsCameraAvailable
        {
            get;
            private set;
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
            get { return false; }
        }
        /// <inheritdoc/>
        public bool IsPickVideoSupported
        {
            get { return false; }
        }

        /// <summary>
        /// Picks a photo from the default gallery
        /// </summary>
        /// <returns>Media file or null if canceled</returns>
        public Task<MediaFile> PickPhotoAsync()
        {

            var ntcs = new TaskCompletionSource<MediaFile>();
            if (Interlocked.CompareExchange(ref completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at at time");

            this.photoChooser.Show();

            return ntcs.Task;
        }

        /// <summary>
        /// Take a photo async with specified options
        /// </summary>
        /// <param name="options">Camera Media Options</param>
        /// <returns>Media file of photo or null if canceled</returns>
        public Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
        {
            if (!IsCameraAvailable)
                throw new NotSupportedException();

            options.VerifyOptions();

            var ntcs = new TaskCompletionSource<MediaFile>(options);
            if (Interlocked.CompareExchange(ref completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at a time");

            this.cameraCapture.Show();

            return ntcs.Task;
        }

        /// <summary>
        /// Picks a video from the default gallery
        /// </summary>
        /// <returns>Media file of video or null if canceled</returns>
        public Task<MediaFile> PickVideoAsync()
        {
            throw new NotSupportedException();

            /*var ntcs = new TaskCompletionSource<MediaFile>();
            if (Interlocked.CompareExchange(ref completionSource, ntcs, null) != null)
              throw new InvalidOperationException("Only one operation can be active at at time");

            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.ViewMode = PickerViewMode.Thumbnail;
            foreach (var filter in SupportedVideoFileTypes)
              picker.FileTypeFilter.Add(filter);

            var file = await picker.PickSingleFileAsync();
            if(file == null)
              return null;

            return new MediaFile(file.Path, () => file.OpenStreamForReadAsync().Result);*/
        }

        /// <summary>
        /// Take a video with specified options
        /// </summary>
        /// <param name="options">Video Media Options</param>
        /// <returns>Media file of new video or null if canceled</returns>
        public Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
        {
            throw new NotSupportedException();
        }

        private readonly CameraCaptureTask cameraCapture = new CameraCaptureTask();
        private readonly PhotoChooserTask photoChooser = new PhotoChooserTask();
        private static TaskCompletionSource<MediaFile> completionSource;


        private void OnPhotoChosen(object sender, PhotoResult photoResult)
        {
            var tcs = Interlocked.Exchange(ref completionSource, null);

            if (photoResult.TaskResult == TaskResult.Cancel)
            {
                tcs.SetResult(null);
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
                        tcs.SetResult(null);

                    break;
            }
        }
    }
}