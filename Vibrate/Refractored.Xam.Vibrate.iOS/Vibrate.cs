using System;

using Plugin.Vibrate.Abstractions;
#if __UNIFIED__
using AudioToolbox;
#else
using MonoTouch.AudioToolbox;
#endif

namespace Plugin.Vibrate
{
  /// <summary>
  /// iOS implementation to vibrate device
  /// </summary>
  public class Vibrate : IVibrate
  {
    /// <summary>
    /// Vibrate device with default length
    /// </summary>
    /// <param name="milliseconds">Ignored (iOS doesn't expose)</param>
    public void Vibration(int milliseconds = 500)
    {
      SystemSound.Vibrate.PlaySystemSound();
    }
  }
}