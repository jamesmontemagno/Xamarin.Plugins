using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Media
{
    /// <summary>
    /// 
    /// </summary>
    public class MediaImplementation : IMedia
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsCameraAvailable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPickPhotoSupported
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPickVideoSupported
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTakePhotoSupported
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTakeVideoSupported
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> Initialize()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<MediaFile> PickPhotoAsync()
        {
            return Task.FromResult<MediaFile>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<MediaFile> PickVideoAsync()
        {
            return Task.FromResult<MediaFile>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
        {
            return Task.FromResult<MediaFile>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
        {
            return Task.FromResult<MediaFile>(null);
        }
    }
}
