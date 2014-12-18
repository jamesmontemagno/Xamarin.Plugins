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

namespace TestAppForms.Droid
{
  [Activity(Label = "TestAppForms", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : AndroidActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      Xamarin.Forms.Forms.Init(this, bundle);
      //Vibrate.Init();
      CrossTextToSpeech.Current.Init();
      ImageCircleRenderer.Init();
      SetPage(App.GetMainPage());
    }
  }
}

