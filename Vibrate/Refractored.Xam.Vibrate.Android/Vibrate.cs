using System;

using Refractored.Xam.Vibrate.Abstractions;
using Android.OS;
using Android.Content;

namespace Refractored.Xam.Vibrate
{
  /// <summary>
  /// Vibration Implentation on Android 
  /// </summary>
  public class Vibrate : IVibrate
  {
    /// <summary>
    /// Vibrate device for specified amount of time
    /// </summary>
    /// <param name="milliseconds">Time in MS (500ms is default).</param>
    public void Vibration(int milliseconds = 500)
    {
      using (var v = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService))
      {
          if ((int)Build.VERSION.SdkInt >= 11)
          {
#if __ANDROID_11__
            if (!v.HasVibrator)
            {
              Console.WriteLine("Android device does not have vibrator.");
              return;
            }
#endif
          }

        if (milliseconds < 0)
          milliseconds = 0;

        try
        {
          v.Vibrate(milliseconds);
        }
        catch (Exception ex)
        {
          Console.WriteLine("Unable to vibrate Android device, ensure VIBRATE permission is set.");
        }
      }

    }
  }
}