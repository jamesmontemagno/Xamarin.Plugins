using Refractored.Xam.Vibrate.Abstractions;
using Refractored.Xam.Forms.Vibrate.iOS;
using System;
using Xamarin.Forms;
using MonoTouch.AudioToolbox;

[assembly:Dependency(typeof(Vibrate))]
namespace Refractored.Xam.Forms.Vibrate.iOS
{
  /// <summary>
  /// Vibration Implementation on iOS
  /// </summary>
  public class Vibrate : IVibrate
  {
    /// <summary>
    /// Initialization code
    /// </summary>
    public static void Init()
    {
    }
    /// <summary>
    /// Vibrate device with default length
    /// </summary>
    /// <param name="milliseconds">Ignored (iOS doesn't expose)</param>
    public void Vibration(int milliseconds = 0)
    {
      SystemSound.Vibrate.PlaySystemSound();
    }
  }
}
