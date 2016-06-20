using System;

using UIKit;
using Plugin.Connectivity;

namespace ConnectivityTest.iOS
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

        bool reg;
        partial void ButtonRegister_TouchUpInside(UIButton sender)
        {
            if(!reg)
            {
                CrossConnectivity.Current.ConnectivityChanged += CrossConnectivity_Current_ConnectivityChanged;
            }
            else
            {    
                CrossConnectivity.Current.ConnectivityChanged -= CrossConnectivity_Current_ConnectivityChanged;
            }

            reg = !reg;
        }

        void CrossConnectivity_Current_ConnectivityChanged (object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            InvokeOnMainThread(() =>
                {
                    new UIAlertView("status", "Connected: " + e.IsConnected + " Connected: " + CrossConnectivity.Current.IsConnected, null, "OK").Show();
                });
        }

        async partial void ButtonStatus_TouchUpInside(UIButton sender)
        {

            var test1 = await CrossConnectivity.Current.IsReachable("montemagno.com");
            var test2 = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            var test3 = await CrossConnectivity.Current.IsRemoteReachable("montemagno.com");
            var test4 = await CrossConnectivity.Current.IsRemoteReachable("xamarinevolve.azurewebsites.net");
            var test5 = await CrossConnectivity.Current.IsRemoteReachable("xamarin2222.com");
            LabelStatus.Text = "connected: " + CrossConnectivity.Current.IsConnected;
        }
    }
}

