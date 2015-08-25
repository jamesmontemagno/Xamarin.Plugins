using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using ImageCircle.Forms.Plugin.iOS;
using MonoTouch.MessageUI;
//using Refractored.Xam.Forms.Vibrate.iOS;

namespace TestAppForms.iOS
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register("AppDelegate")]
  public partial class AppDelegate : UIApplicationDelegate
  {
    // class-level declarations
    UIWindow window;

    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {

            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(43, 132, 211); //bar background
            UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
                {
                    Font = UIFont.FromName("HelveticaNeue-Light", (float)20f),
                    TextColor = UIColor.White
                });
      Forms.Init();
      //Vibrate.Init();
      ImageCircleRenderer.Init();
      window = new UIWindow(UIScreen.MainScreen.Bounds);

      window.RootViewController = App.GetMainPage().CreateViewController();

      window.MakeKeyAndVisible();

      return true;
    }
  }
}
