using Microsoft.Devices;
using Refractored.Xam.Vibrate.Abstractions;
using Refractored.Xam.Forms.Vibrate.WinPhone;
using System;
using Xamarin.Forms;

[assembly:Dependency(typeof(Vibrate))]
namespace Refractored.Xam.Forms.Vibrate.WinPhone
{
  /// <summary>
  /// Vibration implementation on Windows Phone
  /// </summary>
  public class Vibrate : IVibrate
  {
    /// <summary>
    /// Initialization code
    /// </summary>
    public static void Init() { }

    /// <summary>
    /// Vibrate device for set amount of time
    /// </summary>
    /// <param name="milliseconds">Time in MS (min 500) 0 == default.</param>
    public void Vibration(int milliseconds = 0)
    {
      var v = VibrateController.Default;

      if (milliseconds <= 0)
        milliseconds = 500;

      v.Start(TimeSpan.FromMilliseconds(milliseconds));
    }
  }
}
