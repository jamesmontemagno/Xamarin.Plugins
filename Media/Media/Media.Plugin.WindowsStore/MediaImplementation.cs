using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;

using Plugin.Media.Abstractions;
using System.Diagnostics;

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
            if (!initialized)
                await Initialize();

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
            if (!initialized)
                await Initialize();

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

            string aPath = null;
            if (options?.SaveToAlbum ?? false)
            {
                try
                {
                    string fileNameNoEx = Path.GetFileNameWithoutExtension(result.Path);
                    var copy = await result.CopyAsync(KnownFolders.VideosLibrary, fileNameNoEx + result.FileType, NameCollisionOption.GenerateUniqueName);
                    aPath = copy.Path;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("unable to save to album:" + ex);
                }
            }

            return new MediaFile(result.Path, () => result.OpenStreamForReadAsync().Result, albumPath: aPath);
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