using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Plugin.TextToSpeech;
using ImageCircle.Forms.Plugin.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Content;

namespace TestAppForms.Droid
{
    [Activity(Label = "Plugins for Xamarin", MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            CrossTextToSpeech.Current.Init();
            ImageCircleRenderer.Init();

            Xamarin.Insights.Initialize(Helpers.Settings.InsightsKey, this);

            Xamarin.Insights.ForceDataTransmission = true;
            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;
            LoadApplication(new App2());
            
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_action_navigation_arrow_back);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

