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
using System.Threading.Tasks;

using Media.Plugin.Abstractions;

#if __UNIFIED__
using UIKit;
using Foundation;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif

namespace Media.Plugin
{
    /// <summary>
    /// Media Picker Controller
    /// </summary>
    public sealed class MediaPickerController
        : UIImagePickerController
    {
        
        internal MediaPickerController(MediaPickerDelegate mpDelegate)
        {
            base.Delegate = mpDelegate;
         }

        /// <summary>
        /// Deleage
        /// </summary>
        public override NSObject Delegate
        {
            get { return base.Delegate; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets result of picker
        /// </summary>
        /// <returns></returns>
        public Task<MediaFile> GetResultAsync()
        {
            return ((MediaPickerDelegate)Delegate).Task;
        }
    }
}