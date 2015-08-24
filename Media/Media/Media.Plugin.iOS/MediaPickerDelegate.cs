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
using System.Linq;
using System.Threading.Tasks;

using Media.Plugin.Abstractions;

#if __UNIFIED__
using CoreGraphics;
using AssetsLibrary;
using Foundation;
using UIKit;
using NSAction = global::System.Action;
#else
using MonoTouch.AssetsLibrary;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using CGRect = global::System.Drawing.RectangleF;
using nfloat = global::System.Single;
#endif

namespace Media.Plugin
{
    internal class MediaPickerDelegate
        : UIImagePickerControllerDelegate
    {
        internal MediaPickerDelegate(UIViewController viewController, UIImagePickerControllerSourceType sourceType, StoreCameraMediaOptions options)
        {
            this.viewController = viewController;
            this.source = sourceType;
            this.options = options ?? new StoreCameraMediaOptions();

            if (viewController != null)
            {
                UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
                this.observer = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, DidRotate);
            }
        }

        public UIPopoverController Popover
        {
            get;
            set;
        }

        public UIView View
        {
            get { return this.viewController.View; }
        }

        public Task<MediaFile> Task
        {
            get { return tcs.Task; }
        }

        public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            MediaFile mediaFile;
            switch ((NSString)info[UIImagePickerController.MediaType])
            {
                case MediaImplementation.TypeImage:
                    mediaFile = GetPictureMediaFile(info);
                    break;

                case MediaImplementation.TypeMovie:
                    mediaFile = GetMovieMediaFile(info);
                    break;

                default:
                    throw new NotSupportedException();
            }

            Dismiss(picker, () => this.tcs.TrySetResult(mediaFile));
        }

        public override void Canceled(UIImagePickerController picker)
        {
            Dismiss(picker, () => this.tcs.SetResult(null));
        }

        public void DisplayPopover(bool hideFirst = false)
        {
            if (Popover == null)
                return;

            var swidth = UIScreen.MainScreen.Bounds.Width;
            var sheight = UIScreen.MainScreen.Bounds.Height;

            nfloat width = 400;
            nfloat height = 300;

            if (this.orientation == null)
            {
                if (IsValidInterfaceOrientation(UIDevice.CurrentDevice.Orientation))
                    this.orientation = UIDevice.CurrentDevice.Orientation;
                else
                    this.orientation = GetDeviceOrientation(this.viewController.InterfaceOrientation);
            }

            nfloat x, y;
            if (this.orientation == UIDeviceOrientation.LandscapeLeft || this.orientation == UIDeviceOrientation.LandscapeRight)
            {
                y = (swidth / 2) - (height / 2);
                x = (sheight / 2) - (width / 2);
            }
            else
            {
                x = (swidth / 2) - (width / 2);
                y = (sheight / 2) - (height / 2);
            }

            if (hideFirst && Popover.PopoverVisible)
                Popover.Dismiss(animated: false);

            Popover.PresentFromRect(new CGRect(x, y, width, height), View, 0, animated: true);
        }

        private UIDeviceOrientation? orientation;
        private NSObject observer;
        private readonly UIViewController viewController;
        private readonly UIImagePickerControllerSourceType source;
        private readonly TaskCompletionSource<MediaFile> tcs = new TaskCompletionSource<MediaFile>();
        private readonly StoreCameraMediaOptions options;

        private bool IsCaptured
        {
            get { return this.source == UIImagePickerControllerSourceType.Camera; }
        }

        private void Dismiss(UIImagePickerController picker, NSAction onDismiss)
        {
            if (this.viewController == null)
                onDismiss();
            else
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(this.observer);
                UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();

                this.observer.Dispose();

                if (Popover != null)
                {
                    Popover.Dismiss(animated: true);
                    Popover.Dispose();
                    Popover = null;

                    onDismiss();
                }
                else
                {
                    picker.DismissViewController(true, onDismiss);
                    picker.Dispose();
                }
            }
        }

        private void DidRotate(NSNotification notice)
        {
            UIDevice device = (UIDevice)notice.Object;
            if (!IsValidInterfaceOrientation(device.Orientation) || Popover == null)
                return;
            if (this.orientation.HasValue && IsSameOrientationKind(this.orientation.Value, device.Orientation))
                return;

            if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
            {
                if (!GetShouldRotate6(device.Orientation))
                    return;
            }
            else if (!GetShouldRotate(device.Orientation))
                return;

            UIDeviceOrientation? co = this.orientation;
            this.orientation = device.Orientation;

            if (co == null)
                return;

            DisplayPopover(hideFirst: true);
        }

        private bool GetShouldRotate(UIDeviceOrientation orientation)
        {
            UIInterfaceOrientation iorientation = UIInterfaceOrientation.Portrait;
            switch (orientation)
            {
                case UIDeviceOrientation.LandscapeLeft:
                    iorientation = UIInterfaceOrientation.LandscapeLeft;
                    break;

                case UIDeviceOrientation.LandscapeRight:
                    iorientation = UIInterfaceOrientation.LandscapeRight;
                    break;

                case UIDeviceOrientation.Portrait:
                    iorientation = UIInterfaceOrientation.Portrait;
                    break;

                case UIDeviceOrientation.PortraitUpsideDown:
                    iorientation = UIInterfaceOrientation.PortraitUpsideDown;
                    break;

                default: return false;
            }

            return this.viewController.ShouldAutorotateToInterfaceOrientation(iorientation);
        }

        private bool GetShouldRotate6(UIDeviceOrientation orientation)
        {
            if (!this.viewController.ShouldAutorotate())
                return false;

            UIInterfaceOrientationMask mask = UIInterfaceOrientationMask.Portrait;
            switch (orientation)
            {
                case UIDeviceOrientation.LandscapeLeft:
                    mask = UIInterfaceOrientationMask.LandscapeLeft;
                    break;

                case UIDeviceOrientation.LandscapeRight:
                    mask = UIInterfaceOrientationMask.LandscapeRight;
                    break;

                case UIDeviceOrientation.Portrait:
                    mask = UIInterfaceOrientationMask.Portrait;
                    break;

                case UIDeviceOrientation.PortraitUpsideDown:
                    mask = UIInterfaceOrientationMask.PortraitUpsideDown;
                    break;

                default: return false;
            }

            return this.viewController.GetSupportedInterfaceOrientations().HasFlag(mask);
        }

        private MediaFile GetPictureMediaFile(NSDictionary info)
        {
            var image = (UIImage)info[UIImagePickerController.EditedImage];
            if (image == null)
                image = (UIImage)info[UIImagePickerController.OriginalImage];

            string path = GetOutputPath(MediaImplementation.TypeImage,
                options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
                options.Name);

            using (FileStream fs = File.OpenWrite(path))
            using (Stream s = new NSDataStream(image.AsJPEG()))
            {
                s.CopyTo(fs);
                fs.Flush();
            }

            Action<bool> dispose = null;
            if (this.source != UIImagePickerControllerSourceType.Camera)
                dispose = d => File.Delete(path);

            return new MediaFile(path, () => File.OpenRead(path), dispose: dispose);
        }

        private MediaFile GetMovieMediaFile(NSDictionary info)
        {
            NSUrl url = (NSUrl)info[UIImagePickerController.MediaURL];

            string path = GetOutputPath(MediaImplementation.TypeMovie,
                      options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
                      this.options.Name ?? Path.GetFileName(url.Path));

            File.Move(url.Path, path);

            Action<bool> dispose = null;
            if (this.source != UIImagePickerControllerSourceType.Camera)
                dispose = d => File.Delete(path);

            return new MediaFile(path, () => File.OpenRead(path), dispose: dispose);
        }

        private static string GetUniquePath(string type, string path, string name)
        {
            bool isPhoto = (type == MediaImplementation.TypeImage);
            string ext = Path.GetExtension(name);
            if (ext == String.Empty)
                ext = ((isPhoto) ? ".jpg" : ".mp4");

            name = Path.GetFileNameWithoutExtension(name);

            string nname = name + ext;
            int i = 1;
            while (File.Exists(Path.Combine(path, nname)))
                nname = name + "_" + (i++) + ext;

            return Path.Combine(path, nname);
        }

        private static string GetOutputPath(string type, string path, string name)
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
            Directory.CreateDirectory(path);

            if (String.IsNullOrWhiteSpace(name))
            {
                string timestamp = DateTime.Now.ToString("yyyMMdd_HHmmss");
                if (type == MediaImplementation.TypeImage)
                    name = "IMG_" + timestamp + ".jpg";
                else
                    name = "VID_" + timestamp + ".mp4";
            }

            return Path.Combine(path, GetUniquePath(type, path, name));
        }

        private static bool IsValidInterfaceOrientation(UIDeviceOrientation self)
        {
            return (self != UIDeviceOrientation.FaceUp && self != UIDeviceOrientation.FaceDown && self != UIDeviceOrientation.Unknown);
        }

        private static bool IsSameOrientationKind(UIDeviceOrientation o1, UIDeviceOrientation o2)
        {
            if (o1 == UIDeviceOrientation.FaceDown || o1 == UIDeviceOrientation.FaceUp)
                return (o2 == UIDeviceOrientation.FaceDown || o2 == UIDeviceOrientation.FaceUp);
            if (o1 == UIDeviceOrientation.LandscapeLeft || o1 == UIDeviceOrientation.LandscapeRight)
                return (o2 == UIDeviceOrientation.LandscapeLeft || o2 == UIDeviceOrientation.LandscapeRight);
            if (o1 == UIDeviceOrientation.Portrait || o1 == UIDeviceOrientation.PortraitUpsideDown)
                return (o2 == UIDeviceOrientation.Portrait || o2 == UIDeviceOrientation.PortraitUpsideDown);

            return false;
        }

        private static UIDeviceOrientation GetDeviceOrientation(UIInterfaceOrientation self)
        {
            switch (self)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    return UIDeviceOrientation.LandscapeLeft;
                case UIInterfaceOrientation.LandscapeRight:
                    return UIDeviceOrientation.LandscapeRight;
                case UIInterfaceOrientation.Portrait:
                    return UIDeviceOrientation.Portrait;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return UIDeviceOrientation.PortraitUpsideDown;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
