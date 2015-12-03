using Plugin.Media;
using System;

using UIKit;

namespace MediaTest.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TakePhoto.TouchUpInside += async (sender, args) =>
            {
                var test = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Name = "test1.jpg"
                });

                if (test != null)
                    test.Dispose();
            };

            PickPhoto.TouchUpInside += async (sender, args) =>
            {
                var test = await CrossMedia.Current.PickPhotoAsync();
                if (test != null)
                    test.Dispose();
            };

            TakeVideo.TouchUpInside += async (sender, args) =>
            {
                var test = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                {
                    Name = "test1.mp4"
                });
                if (test != null)
                    test.Dispose();
            };

            PickVideo.TouchUpInside += async (sender, args) =>
            {
                var test = await CrossMedia.Current.PickVideoAsync();
                if (test != null)
                    test.Dispose();
            };
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}