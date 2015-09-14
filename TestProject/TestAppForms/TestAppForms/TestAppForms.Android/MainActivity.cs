using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
//using Refractored.Xam.Forms.Vibrate.Droid;
using Refractored.Xam.TTS;
using ImageCircle.Forms.Plugin.Droid;
using Android.Graphics.Drawables;

namespace TestAppForms.Droid
{
    [Activity(Label = "Plugins for Xamarin", MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : FormsApplicationActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      Xamarin.Forms.Forms.Init(this, bundle);
      //Vibrate.Init();
      CrossTextToSpeech.Current.Init();
      ImageCircleRenderer.Init();
            Xamarin.Insights.Initialize(TestAppForms.Helpers.Settings.InsightsKey, this);
            Xamarin.Insights.ForceDataTransmission = true;
            LoadApplication(new App2());

            if ((int)Android.OS.Build.VERSION.SdkInt >= 21)
            {
                ActionBar.SetIcon(
                    new ColorDrawable(Resources.GetColor(Android.Resource.Color.Transparent)));
            }
    }
  }
}

