using System;

using Plugin.Vibrate.Abstractions;
#if NETFX_CORE
using Windows.Phone.Devices.Notification;
#else
using Microsoft.Devices;
#endif


namespace Plugin.Vibrate
{
  /// <summary>
  /// Windows Phone implemenation of Vibrate
  /// </summary>
  public class Vibrate : IVibrate
  {

    /// <summary>
    /// Vibrate device for set amount of time
    /// </summary>
    /// <param name="milliseconds">Time in MS (500ms is default).</param>
    public void Vibration(int milliseconds = 500)
    {
#if NETFX_CORE
      var v = VibrationDevice.GetDefault();
#else
      var v = VibrateController.Default;
#endif

      if (milliseconds < 0)
        milliseconds = 0;
      else if (milliseconds > 5000)
        milliseconds = 5000;

      var time = TimeSpan.FromMilliseconds(milliseconds);
#if NETFX_CORE
      v.Vibrate(time);
#else
      v.Start(time);
#endif
    }
  }
}