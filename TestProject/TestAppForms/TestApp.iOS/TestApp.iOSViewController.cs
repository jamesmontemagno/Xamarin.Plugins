using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TestApp.iOS
{
  public partial class TestAppiOSViewController : UIViewController
  {
    public TestAppiOSViewController(IntPtr handle)
      : base(handle)
    {
    }

    public override void DidReceiveMemoryWarning()
    {
      // Releases the view if it doesn't have a superview.
      base.DidReceiveMemoryWarning();

      // Release any cached data, images, etc that aren't in use.
    }

    #region View lifecycle

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      ButtonDoStuff.TouchUpInside += (sender, args) =>
      {
        Settings.GeneralSettings = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(); ;
        LabelDoStuff.Text = Settings.GeneralSettings;

        Settings.GeneralSettings2 = !Settings.GeneralSettings2;
        LabelDoStuff.Text += "Success";

      };
      // Perform any additional setup after loading the view, typically from a nib.
    }

    public override void ViewWillAppear(bool animated)
    {
      base.ViewWillAppear(animated);
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);
    }

    public override void ViewWillDisappear(bool animated)
    {
      base.ViewWillDisappear(animated);
    }

    public override void ViewDidDisappear(bool animated)
    {
      base.ViewDidDisappear(animated);
    }

    #endregion
  }
}