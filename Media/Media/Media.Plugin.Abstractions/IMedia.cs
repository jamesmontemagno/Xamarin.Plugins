using System;
using System.Threading.Tasks;

namespace Media.Plugin.Abstractions
{
  /// <summary>
  /// Interface for Media
  /// </summary>
  public interface IMedia
  {
    bool IsCameraAvailable { get; }
    bool PhotosSupported { get; }
    bool VideosSupported { get; }

    Task<MediaFile> PickPhotoAsync();

    Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options);

    Task<MediaFile> PickVideoAsync();

    Task<MediaFile> TakeVideoAsync(StoreVideoOptions options);

  }
}
