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
using System.Threading.Tasks;
using Android.Content;
using File = System.IO.File;
using IOException = System.IO.IOException;
using Media.Plugin.Abstractions;

namespace Media.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public static class MediaFileExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Task<MediaFile> GetMediaFileExtraAsync(this Intent self, Context context)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (context == null)
                throw new ArgumentNullException("context");

            string action = self.GetStringExtra("action");
            if (action == null)
                throw new ArgumentException("Intent was not results from MediaPicker", "self");

            var uri = (Android.Net.Uri)self.GetParcelableExtra("MediaFile");
            bool isPhoto = self.GetBooleanExtra("isPhoto", false);
            var path = (Android.Net.Uri)self.GetParcelableExtra("path");

            return MediaPickerActivity.GetMediaFileAsync(context, 0, action, isPhoto, ref path, uri)
                .ContinueWith(t => t.Result.ToTask()).Unwrap();
        }
    }

}