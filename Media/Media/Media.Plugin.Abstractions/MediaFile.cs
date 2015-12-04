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


namespace Plugin.Media.Abstractions
{
    /// <summary>
    /// Media file representations
    /// </summary>
    public sealed class MediaFile
      : IDisposable
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="streamGetter"></param>
        /// <param name="deletePathOnDispose"></param>
        /// <param name="dispose"></param>
        public MediaFile(string path, Func<Stream> streamGetter, bool deletePathOnDispose = false, Action<bool> dispose = null, string albumPath = null)
        {
            this.dispose = dispose;
            this.streamGetter = streamGetter;
            this.path = path;
            this.deletePathOnDispose = deletePathOnDispose;
            this.albumPath = albumPath;
        }
        /// <summary>
        /// Path to file
        /// </summary>
        public string Path
        {
            get
            {
                if (isDisposed)
                    throw new ObjectDisposedException(null);

                return path;
            }
        }

        /// <summary>
        /// Path to file
        /// </summary>
        public string AlbumPath
        {
            get
            {
                if (isDisposed)
                    throw new ObjectDisposedException(null);

                return albumPath;
            }
        }

        /// <summary>
        /// Get stream if available
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);

            return streamGetter();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed;
        private readonly Action<bool> dispose;
        private readonly Func<Stream> streamGetter;
        private readonly string path, albumPath;
        private readonly bool deletePathOnDispose;

        private void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            isDisposed = true;
            if (dispose != null)
                dispose(disposing);
        }
        /// <summary>
        /// 
        /// </summary>
        ~MediaFile()
        {
            Dispose(false);
        }
    }
}
