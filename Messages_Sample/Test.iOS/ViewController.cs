using System;

using UIKit;
using Test.Portable;

namespace Test.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void UIButton5_TouchUpInside(UIButton sender)
        {
            Class1.ShowMessage("Hello iOS");
        }

        partial void UIButton6_TouchUpInside(UIButton sender)
        {
            Class1.ShowMessageEx("Hello from iOS", "Longer message");
        }
    }
}

