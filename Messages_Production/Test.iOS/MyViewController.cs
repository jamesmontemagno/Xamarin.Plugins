using System;
using MonoTouch.UIKit;
using System.Drawing;
using Test.Portable;

namespace Test.iOS
{
  public class MyViewController : UIViewController
  {
    UIButton button, button2;
    int numClicks = 0;
    float buttonWidth = 200;
    float buttonHeight = 50;

    public MyViewController()
    {
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      View.Frame = UIScreen.MainScreen.Bounds;
      View.BackgroundColor = UIColor.White;
      View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

      button = UIButton.FromType(UIButtonType.RoundedRect);

      button.Frame = new RectangleF(
          View.Frame.Width / 2 - buttonWidth / 2,
          View.Frame.Height / 2 - buttonHeight / 2,
          buttonWidth,
          buttonHeight);

      button.SetTitle("Click me", UIControlState.Normal);

      button.TouchUpInside += (object sender, EventArgs e) =>
      {
        Class1.ShowMessage("from iOS");
        button.SetTitle(String.Format("clicked {0} times", numClicks++), UIControlState.Normal);
        
      };

      button.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin |
          UIViewAutoresizing.FlexibleBottomMargin;

      button2 = UIButton.FromType(UIButtonType.RoundedRect);

      button2.Frame = new RectangleF(
          View.Frame.Width / 2 - buttonWidth / 2,
          (View.Frame.Height / 2 - buttonHeight / 2) + buttonHeight+10,
          buttonWidth,
          buttonHeight);

      button2.SetTitle("Click me", UIControlState.Normal);

      button2.TouchUpInside += (object sender, EventArgs e) =>
      {
        Class1.ShowMessageEx("from iOS", "longer message");
        button.SetTitle(String.Format("clicked {0} times", numClicks++), UIControlState.Normal);

      };

      button2.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin |
          UIViewAutoresizing.FlexibleBottomMargin;

      View.AddSubview(button);
    }

  }
}

