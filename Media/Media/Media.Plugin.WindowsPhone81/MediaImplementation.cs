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

using Plugin.Media.Abstractions;
using Windows.UI.Xaml.Controls;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml;
using System.Threading;
using System.Linq;
using Windows.ApplicationModel.Activation;
using DMX.Helper;
using System.Diagnostics;

namespace Plugin.Media
{
    /// <summary>
    /// Implementation for Media
    /// </summary>
    public class MediaImplementation : IMedia
    {

        private static TaskCompletionSource<MediaFile> completionSource;
        private static readonly IEnumerable<string> SupportedVideoFileTypes = new List<string> { ".mp4", ".wmv", ".avi" };
        private static readonly IEnumerable<string> SupportedImageFileTypes = new List<string> { ".jpeg", ".jpg", ".png", ".gif", ".bmp" };

        /// <summary>
        /// Implementation
        /// </summary>
        public MediaImplementation()
        {


            watcher = DeviceInformation.CreateWatcher(DeviceClass.VideoCapture);
            watcher.Added += OnDeviceAdded;
            watcher.Updated += OnDeviceUpdated;
            watcher.Removed += OnDeviceRemoved;
            watcher.Start();
        }

        bool initialized = false;
        public async Task<bool> Initialize()
        {
            try
            {
                var info = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture).AsTask().ConfigureAwait(false);
                lock (devices)
                {
                    foreach (var device in info)
                    {
                        if (device.IsEnabled)
                            devices.Add(device.Id);
                    }

                    isCameraAvailable = (devices.Count > 0);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to detect cameras: " + ex);
            }

            initialized = true;
            return true;
        }

        /// <inheritdoc/>
        public bool IsCameraAvailable
        {
            get
            {
                if (!initialized)
                    Initialize().Wait();

                return isCameraAvailable;
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
            get { return false; }
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
            if (!initialized)
                await Initialize();

            if (!IsCameraAvailable)
                throw new NotSupportedException();

            options.VerifyOptions();

            var capture = new CameraCaptureUI();
            var result = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo, options);
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
            string aPath = null;
            if (options?.SaveToAlbum ?? false)
            {
                try
                {
                    string fileNameNoEx = Path.GetFileNameWithoutExtension(path);
                    var copy = await result.CopyAsync(KnownFolders.PicturesLibrary, fileNameNoEx + result.FileType, NameCollisionOption.GenerateUniqueName);
                    aPath = copy.Path;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("unable to save to album:" + ex);
                }
            }

            var file = await result.CopyAsync(folder, filename, NameCollisionOption.GenerateUniqueName).AsTask();
            return new MediaFile(file.Path, () => file.OpenStreamForReadAsync().Result, albumPath: aPath);
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


            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.ViewMode = PickerViewMode.Thumbnail;
            foreach (var filter in SupportedImageFileTypes)
                picker.FileTypeFilter.Add(filter);

            picker.PickSingleFileAndContinue();
            return ntcs.Task;
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


        /// <summary>
        /// Picks a video from the default gallery
        /// </summary>
        /// <returns>Media file of video or null if canceled</returns>
        public Task<MediaFile> PickVideoAsync()
        {
            var ntcs = new TaskCompletionSource<MediaFile>();
            if (Interlocked.CompareExchange(ref completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at at time");

            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.ViewMode = PickerViewMode.Thumbnail;
            foreach (var filter in SupportedVideoFileTypes)
                picker.FileTypeFilter.Add(filter);

            picker.PickSingleFileAndContinue();
            return ntcs.Task;
        }

        private readonly HashSet<string> devices = new HashSet<string>();
        private readonly DeviceWatcher watcher;
        private bool isCameraAvailable;


        /// <summary>
        /// OnFilesPicked
        /// </summary>
        /// <param name="args"></param>
        public static void OnFilesPicked(IActivatedEventArgs args)
        {
            var tcs = Interlocked.Exchange(ref completionSource, null);


            IReadOnlyList<StorageFile> files;
            var fopArgs = args as FileOpenPickerContinuationEventArgs;
            if (fopArgs != null)
            {

                // Pass the picked files to the subscribed event handlers
                // In a real world app you could also use a Messenger, Listener or any other subscriber-based model
                if (fopArgs.Files.Any())
                {
                    files = fopArgs.Files;
                }
                else
                {
                    tcs.SetResult(null);
                    return;
                }
            }
            else
            {
                tcs.SetResult(null);
                return;

            }


            // Check if video or image and pick first file to show
            var imageFile = files.FirstOrDefault(f => SupportedImageFileTypes.Contains(f.FileType.ToLower()));
            if (imageFile != null)
            {
                tcs.SetResult(new MediaFile(imageFile.Path, () => imageFile.OpenStreamForReadAsync().Result));
                return;
            }

            var videoFile = files.FirstOrDefault(f => SupportedVideoFileTypes.Contains(f.FileType.ToLower()));
            if (videoFile != null)
            {
                tcs.SetResult(new MediaFile(videoFile.Path, () => videoFile.OpenStreamForReadAsync().Result));
                return;
            }

            tcs.SetResult(null);

        }


        private void OnDeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate update)
        {
            object value;
            if (!update.Properties.TryGetValue("System.Devices.InterfaceEnabled", out value))
                return;

            lock (devices)
            {
                if ((bool)value)
                    devices.Add(update.Id);
                else
                    devices.Remove(update.Id);

                isCameraAvailable = devices.Count > 0;
            }
        }

        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate update)
        {
            lock (devices)
            {
                devices.Remove(update.Id);
                if (devices.Count == 0)
                    isCameraAvailable = false;
            }
        }

        private void OnDeviceAdded(DeviceWatcher sender, DeviceInformation device)
        {
            if (!device.IsEnabled)
                return;

            lock (devices)
            {
                devices.Add(device.Id);
                isCameraAvailable = true;
            }
        }
    }
}