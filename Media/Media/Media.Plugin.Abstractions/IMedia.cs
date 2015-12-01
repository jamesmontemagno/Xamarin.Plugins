using System;
using System.Threading.Tasks;

namespace Plugin.Media.Abstractions
{
    /// <summary>
    /// Interface for Media
    /// </summary>
    public interface IMedia
    {
        /// <summary>
        /// Initialize all camera components
        /// </summary>
        /// <returns></returns>
        Task<bool> Initialize();
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

    }
}
