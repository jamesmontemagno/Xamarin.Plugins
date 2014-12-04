using Android.Content;
using Android.OS;
using Refractored.Xam.Vibrate.Abstractions;
using Refractored.Xam.Forms.Vibrate.Droid;
using System;
using Xamarin.Forms;

[assembly:Dependency(typeof(Vibrate))]
namespace Refractored.Xam.Forms.Vibrate.Droid
{
  /// <summary>
  /// Vibration Implentation on Android 
  /// </summary>
  public class Vibrate : IVibrate
  {
    /// <summary>
    /// Initialization code for Vibrate
    /// </summary>
    public static void Init()
    {
    }
    /// <summary>
    /// Vibrate device for specified amount of time
    /// </summary>
    /// <param name="milliseconds">Time in MS (min 500) 0 is default.</param>
    public void Vibration(int milliseconds = 0)
    {
      var v = (Vibrator)Xamarin.Forms.Forms.Context.GetSystemService(Context.VibratorService);

      if (!v.HasVibrator){
        Console.WriteLine("Android device does not have vibrator.");
        return;
      }

      if (milliseconds <= 0)
        milliseconds = 500; // default and minimum

      try {
        v.Vibrate(milliseconds);
      }
      catch(Exception ex)
      {
        Console.WriteLine("Unable to vibrate Android device, ensure VIBRATE permission is set.");
      }
    }
  }
}
