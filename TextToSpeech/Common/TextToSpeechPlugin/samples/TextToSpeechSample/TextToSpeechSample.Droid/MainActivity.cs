using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TextToSpeechSample.Droid
{
  [Activity(Label = "TextToSpeechSample", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      global::Xamarin.Forms.Forms.Init(this, bundle);
      //Not required, but recommended
      Refractored.Xam.TTS.CrossTextToSpeech.Current.Init();
      LoadApplication(new App());
    }
  }
}

